using NUnit.Framework;
using Reqnroll;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FluentAssertions;
using tests.support;
using CardValidation.ViewModels;

namespace tests.step_definitions
{
    [Binding] // 🚀 REQUIRED for Reqnroll to detect this class
    public class CardValidationSteps
    {
        private readonly support.TestContext _context;
        private HttpResponseMessage _response;
        private string _endpoint = "/CardValidation/card/credit/validate";
        private CreditCard _creditCard;

        public CardValidationSteps(NUnit.Framework.TestContext context)
        {
            _context = context;
        }

        [Given(@"the API base URL is set")]
        public void GivenTheApiBaseUrlIsSet()
        {
            _context.SetBaseUrl("https://api.example.com");
        }

        [Given(@"a valid Visa credit card")]
        public void GivenAValidVisaCreditCard()
        {
            _creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "12/25",
                Cvv = "123"
            };
        }

        [Given(@"a valid MasterCard with an incorrect CVC")]
        public void GivenAValidMasterCardWithAnIncorrectCVC()
        {
            _creditCard = new CreditCard
            {
                Owner = "Jane Smith",
                Number = "5555555555554444",
                Date = "11/26",
                Cvv = "12"
            };
        }

        [Given(@"a credit card with missing owner name")]
        public void GivenACreditCardWithMissingOwnerName()
        {
            _creditCard = new CreditCard
            {
                Owner = "",
                Number = "4111111111111111",
                Date = "12/25",
                Cvv = "123"
            };
        }

        [Given(@"an invalid card number ""(.*)""")]
        public void GivenAnInvalidCardNumber(string number)
        {
            _creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = number,
                Date = "12/25",
                Cvv = "123"
            };
        }

        [When(@"I send a POST request to ""(.*)""")]
        public async Task WhenISendAPostRequestTo(string endpoint)
        {
            _response = await _context.ApiClient.PostAsync(endpoint, _creditCard);
        }

        [Then(@"the response status should be (.*)")]
        public void ThenTheResponseStatusShouldBe(int expectedStatusCode)
        {
            ((int)_response.StatusCode).Should().Be(expectedStatusCode);
        }

        [Then(@"the response should return ""(.*)""")]
        public async Task ThenTheResponseShouldReturn(string expectedPaymentSystem)
        {
            var content = await _response.Content.ReadAsStringAsync();
            content.Should().Contain(expectedPaymentSystem);
        }

        [Then(@"the response should contain an error message ""(.*)""")]
        public async Task ThenTheResponseShouldContainAnErrorMessage(string expectedMessage)
        {
            var content = await _response.Content.ReadAsStringAsync();
            var responseBody = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
            responseBody.Should().ContainKey("error");
            responseBody["error"].Should().Be(expectedMessage);
        }
    }
}
