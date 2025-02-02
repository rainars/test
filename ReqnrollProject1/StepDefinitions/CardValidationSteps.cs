using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Reqnroll;
using CardValidation.ViewModels;
using tests.support;
using TechTalk.SpecFlow;  // Assuming SpecFlow is being used
using TestContext = tests.support.TestContext;
using GivenAttribute = Reqnroll.GivenAttribute;
using ThenAttribute = Reqnroll.ThenAttribute;

namespace tests.step_definitions
{
    [Reqnroll.Binding]
    public class CardValidationSteps
    {
        private readonly TestContext _context;
        private HttpResponseMessage _response = default!;
        private CreditCard _creditCard = default!;

        public CardValidationSteps(TestContext context)
        {
            _context = context;
        }

        #region Given Steps

        [Reqnroll.Given(@"the API base URL is set")]
        public void GivenTheApiBaseUrlIsSet()
        {
            _context.SetBaseUrl("http://localhost:7135");
        }

        [Given(@"a valid Visa credit card with number ""(.*)""")]
        public void GivenAValidVisaCreditCard(string cardNumber)
        {
            _creditCard = CreateCreditCard("John Doe", cardNumber, "12/25", "123");
        }

        [Given(@"a valid MasterCard credit card")]
        public void GivenAValidMasterCreditCard()
        {
            _creditCard = CreateCreditCard("John Doe", "5555555555554444", "12/25", "123");
        }

           
        [Given(@"a credit card with issue date ""(.*)""")]
        public void GivenACreditCardWithIssueDate(string issueDate)
        {
            // Using a Visa card number as an example
            _creditCard = CreateCreditCard("John Doe", "4111111111111111", issueDate, "123");
        }

        [Given(@"a credit card with a missing owner and number ""(.*)""")]
        public void GivenACreditCardWithAMissingOwnerAndNumber(string cardNumber)
        {
            _creditCard = CreateCreditCard(string.Empty, cardNumber, "12/25", "123");
        }

        [Given(@"a valid MasterCard with an incorrect CVC ""(.*)""")]
        public void GivenAValidMasterCardWithAnIncorrectCvc(string cvvNumber)
        {
            _creditCard = CreateCreditCard("Jane Smith", "5555555555554444", "11/26", cvvNumber);
        }

        [Given(@"an invalid credit card number")]
        public void GivenAnInvalidCreditCardNumber()
        {
            _creditCard = CreateCreditCard("Invalid Owner", "1234567890123456", "01/30", "999");
        }


        [Given(@"^a credit card with ONLY owner ""(.*)"" and number ""(.*)""$")]
        public void GivenACreditCardWithOnlyOwner(string owner, string number)
        {
            _creditCard = new CreditCard
            {
                Owner = owner.Trim(),
                Number = number.Trim(),
                Date = "12/25",
                Cvv = "123"
            };
        }




        [Given(@"^a credit card with owner\s*""(.*)""\s*and\s*CVC\s*""(.*)""$")]
        public void GivenACreditCardWithOwnerAndCvc(string owner, string cvv)
        {
            _creditCard = new CreditCard
            {
                Owner = owner.Trim(),
                Number = "4111111111111111",
                Date = "12/25",
                Cvv = cvv.Trim()
            };
        }


        [Reqnroll.When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPostRequestTo(string endpoint)
        {
            var jsonPayload = JsonConvert.SerializeObject(_creditCard);
            Console.WriteLine($"POST Payload:\n{jsonPayload}\n");
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            _response = await _context.ApiClient.PostAsync(endpoint, httpContent);

            _response.Should().NotBeNull("The API response should not be null.");
        }


        [Reqnroll.Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            ((int)_response.StatusCode)
                .Should()
                .Be(expectedStatusCode, $"Expected status code {expectedStatusCode}, but got {(int)_response.StatusCode}.");
        }

        [Reqnroll.Then(@"the response should contain ""(.*)""")]
        public async Task ThenTheResponseShouldContain(string expectedMessage)
        {
            var content = await GetResponseContentAsync();

            if (TryParseJson(content, out var jsonResponse))
            {
                bool messageFound = jsonResponse
                    .Descendants()
                    .OfType<JValue>()
                    .Any(value => value.ToString().Contains(expectedMessage));
                messageFound.Should().BeTrue(
                    $"Expected to find '{expectedMessage}' in the JSON response, but got: {content}");
            }
            else
            {
                content.Trim().Should().Be(expectedMessage,
                    $"Expected response content to be '{expectedMessage}', but got '{content.Trim()}'");
            }
        }

        [Given(@"a credit card with CVC ""(.*)""")]
        public void GivenACreditCardWithCVC(string cvc)
        {
            _creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",  // Example Visa card (valid 16 digits)
                Date = "12/25",
                Cvv = cvc  // Dynamically assigned CVC
            };
        }


        [Then(@"the response should contain errors:")]
        public async Task ThenTheResponseShouldContainErrors(Reqnroll.Table table)
        {
            var content = await _response.Content.ReadAsStringAsync();

            try
            {
                var jsonResponse = JObject.Parse(content);

                foreach (var row in table.Rows)
                {
                    var field = row["field"];
                    var expectedMessage = row["message"];

                    // Check if the response contains the field
                    if (jsonResponse.TryGetValue(field, StringComparison.OrdinalIgnoreCase, out JToken errorMessages))
                    {
                        // Handle cases where the response might have multiple error messages for the same field
                        var messages = errorMessages.Type == JTokenType.Array
                            ? errorMessages.ToObject<List<string>>()
                            : new List<string> { errorMessages.ToString() };

                        // Validate that at least one message contains the expected message
                        bool messageFound = messages.Any(message => message.Contains(expectedMessage));

                        if (!messageFound)
                        {
                            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(
                                $"Expected error message '{expectedMessage}' for field '{field}' not found in messages: {string.Join(", ", messages)}");
                        }
                    }
                    else
                    {
                        Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail(
                            $"Expected field '{field}' not found in the response. Full response: {content}");
                    }
                }
            }
            catch (JsonReaderException)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"Failed to parse JSON response. Raw response: {content}");
            }
        }


        [Reqnroll.Then(@"the response should contain an error for Owner ""(.*)""")]
        public async Task ThenTheThenTheResponseShouldContainOwnerError(string expectedMessage)
        {
            var content = await GetResponseContentAsync();

            if (TryParseJson(content, out var jsonResponse))
            {
                // Specifically look for errors under the "Owner" field
                if (jsonResponse.TryGetValue("Owner", out var ownerErrors))
                {
                    ownerErrors.ToString().Should().Contain(expectedMessage,
                        $"Expected to find '{expectedMessage}' under 'Owner' errors in the response.");
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"No 'Owner' field found in response. Full response: {content}");
                }
            }
            else
            {
                content.Trim().Should().Be(expectedMessage,
                    $"Expected response content to be '{expectedMessage}', but got '{content.Trim()}'");
            }
        }

        /// <summary>
        /// Creates a CreditCard instance with the provided details.
        /// </summary>
        private CreditCard CreateCreditCard(string owner, string number, string date, string cvv)
        {
            return new CreditCard
            {
                Owner = owner,
                Number = number,
                Date = date,
                Cvv = cvv
            };
        }

        [Then(@"the response should contain an error for Number ""(.*)""")]
        public async Task ThenTheResponseShouldContainAnErrorForNumber(string expectedMessage)
        {
            var content = await _response.Content.ReadAsStringAsync();

            try
            {
                // Parse the response as JSON
                var jsonResponse = JObject.Parse(content);

                // Check if the "Number" field contains the expected error message
                if (jsonResponse.TryGetValue("Number", out JToken numberErrors))
                {
                    numberErrors.ToString().Should().Contain(expectedMessage,
                        $"Expected 'Number' field to contain '{expectedMessage}', but got '{numberErrors}'");
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"No 'Number' field found in the response: {content}");
                }
            }
            catch (JsonReaderException)
            {
                // Handle cases where the response is plain text and not JSON
                content.Trim().Should().Be(expectedMessage,
                    $"Expected response content to be '{expectedMessage}', but got '{content.Trim()}'");
            }
        }


        [Then(@"the response should contain errors for owner ""(.*)"" and CVC ""(.*)""")]
        public async Task ThenTheResponseShouldContainErrorsForOwnerAndCvc(string expectedOwnerError, string expectedCvcError)
        {
            var content = await _response.Content.ReadAsStringAsync();

            try
            {
                var jsonResponse = JObject.Parse(content);

                // Validate owner error
                if (jsonResponse.TryGetValue("Owner", out JToken ownerErrors))
                {
                    ownerErrors.ToString().Should().Contain(expectedOwnerError, $"Expected error '{expectedOwnerError}' for owner");
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"Expected 'Owner' field not found in the response. Full response: {content}");
                }

                // Validate CVC error
                if (jsonResponse.TryGetValue("Cvv", out JToken cvvErrors))
                {
                    cvvErrors.ToString().Should().Contain(expectedCvcError, $"Expected error '{expectedCvcError}' for CVC");
                }
                else
                {
                    Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"Expected 'Cvv' field not found in the response. Full response: {content}");
                }
            }
            catch (JsonReaderException)
            {
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.Fail($"Failed to parse JSON response. Raw response: {content}");
            }
        }


        /// <summary>
        /// Reads the HTTP response content as a string.
        /// </summary>
        private async Task<string> GetResponseContentAsync()
        {
            return await _response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Tries to parse the provided content as JSON.
        /// </summary>
        /// <param name="content">The response content as a string.</param>
        /// <param name="jsonResponse">The parsed JObject if successful; otherwise, null.</param>
        /// <returns>True if parsing succeeded; otherwise, false.</returns>
        private bool TryParseJson(string content, out JObject? jsonResponse)
        {
            try
            {
                jsonResponse = JObject.Parse(content);
                return true;
            }
            catch (JsonReaderException)
            {
                jsonResponse = null;
                return false;
            }
        }

        #endregion
    }
}
