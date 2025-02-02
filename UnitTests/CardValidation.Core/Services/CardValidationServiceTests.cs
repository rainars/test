using CardValidation.Core.Services;
using NUnit.Framework;
using CardValidation.Core.Enums;

namespace UnitTests.CardValidation.Core.Services
{
    [TestFixture]
    public class CardValidationServiceTests
    {
        private CardValidationService _service;

        [SetUp]
        public void Setup()
        {
            _service = new CardValidationService();
        }

        [Test]
        public void ValidateOwner_ShouldReturnTrue_WhenOwnerIsValid()
        {
            // Arrange
            var owner = "John Doe";

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsTrue(result, "The owner name should be considered valid.");
        }

        [TestCase("John123")]
        [TestCase("John_Doe")]
        [TestCase("")]
        [TestCase(" ")]
       
        public void ValidateOwner_ShouldReturnFalse_WhenOwnerIsInvalid(string owner)
        {
            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.That(result, Is.False, $"The owner name '{owner}' should be considered invalid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnFalse_WhenOwnerIsEmpty()
        {
            // Arrange
            string owner = string.Empty;

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.That(result, Is.False, "Empty owner name should be considered invalid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnTrue_WhenOwnerIsExactlyMaxLength()
        {
            // Arrange
            var owner = new string('A', 100);

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsTrue(result, "Owner name of exactly max length should be considered valid.");
        }

        [TestCase("john doe", Description = "Lowercase should be allowed.")]
        [TestCase("JOHN DOE", Description = "Uppercase should be allowed.")]
        [TestCase("John Mary Lee", Description = "Names with multiple words should be allowed.")]
        public void ValidateOwner_ShouldReturnTrue_ForValidOwnerNames(string owner)
        {
            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsTrue(result, $"The owner name '{owner}' should be considered valid.");
        }

        //[TestCase("01/23", true)]
        [TestCase("12/25", true)]
        [TestCase("13/23", false)]
        [TestCase("00/23", false)]
        [TestCase("02/1999", false)]
        [TestCase("02/2030", true)]
        [TestCase("02/30", true)]
        public void ValidateIssueDate_ShouldReturnExpectedResult(string issueDate, bool expectedResult)
        {
            // Act
            var result = _service.ValidateIssueDate(issueDate);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("123", true)]
        [TestCase("1234", true)]
        [TestCase("12", false)]
        [TestCase("12345", false)]
        [TestCase("abc", false)]
        public void ValidateCvc_ShouldReturnExpectedResult(string cvc, bool expectedResult)
        {
            // Act
            var result = _service.ValidateCvc(cvc);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("4111111111111111", true)] // Visa
        [TestCase("5500000000000004", true)] // MasterCard
        [TestCase("340000000000009", true)] // American Express
        [TestCase("1234567890123456", false)] // Invalid card number
        [TestCase("411111111111111", false)] // Invalid length
        [TestCase("55000000000000041", false)] // Invalid length
        public void ValidateNumber_ShouldReturnExpectedResult(string cardNumber, bool expectedResult)
        {
            // Act
            var result = _service.ValidateNumber(cardNumber);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [TestCase("4111111111111111", PaymentSystemType.Visa)]
        [TestCase("5500000000000004", PaymentSystemType.MasterCard)]
        [TestCase("340000000000009", PaymentSystemType.AmericanExpress)]
        public void GetPaymentSystemType_ShouldReturnExpectedResult(string cardNumber, PaymentSystemType expectedResult)
        {
            // Act
            var result = _service.GetPaymentSystemType(cardNumber);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetPaymentSystemType_ShouldThrowNotImplementedException_WhenCardNumberIsInvalid()
        {
            // Arrange
            var cardNumber = "1234567890123456";

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(cardNumber));
        }
    }
}
