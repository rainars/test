using NUnit.Framework;
using CardValidation.Core.Services;
using CardValidation.Core.Enums;

namespace CardValidation.Tests.Services
{
    [TestFixture]
    public class PaymentSystemTypeTests
    {
        private CardValidationService _service;

        [SetUp]
        public void Setup()
        {
            _service = new CardValidationService();
        }

        [Test]
        public void GetPaymentSystemType_ShouldReturnVisa_WhenVisaCardNumberIsGiven()
        {
            // Arrange
            var visaCardNumber = "4111111111111111";  // Valid Visa card number

            // Act
            var result = _service.GetPaymentSystemType(visaCardNumber);

            // Assert
            Assert.That(result, Is.EqualTo(PaymentSystemType.Visa), "Expected Visa as the payment system type.");
        }

        [Test]
        public void GetPaymentSystemType_ShouldReturnMasterCard_WhenMasterCardNumberIsGiven()
        {
            // Arrange
            var masterCardNumber = "5555555555554444";  // Valid MasterCard number

            // Act
            var result = _service.GetPaymentSystemType(masterCardNumber);

            // Assert
            Assert.That(result, Is.EqualTo(PaymentSystemType.MasterCard), "Expected MasterCard as the payment system type.");
        }

        [Test]
        public void GetPaymentSystemType_ShouldReturnAmericanExpress_WhenAmexCardNumberIsGiven()
        {
            // Arrange
            var amexCardNumber = "378282246310005";  // Valid American Express card number

            // Act
            var result = _service.GetPaymentSystemType(amexCardNumber);

            // Assert
            Assert.That(result, Is.EqualTo(PaymentSystemType.AmericanExpress), "Expected American Express as the payment system type.");
        }

        [Test]
        public void GetPaymentSystemType_ShouldThrowNotImplementedException_WhenUnknownCardNumberIsGiven()
        {
            // Arrange
            var unknownCardNumber = "0000111122223333";  // Invalid card number

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(unknownCardNumber),
                "Expected NotImplementedException for an unknown card number.");
        }
    }
}
