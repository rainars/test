using NUnit.Framework;
using CardValidation.Core.Services;
using CardValidation.Core.Enums;
using System;

namespace unitTests
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
        public void ValidateVisaCardNumber_ShouldReturnTrue_ForValidVisa()
        {
            // Arrange
            string validVisaCard = "4111111111111111";

            // Act
            var result = _service.ValidateNumber(validVisaCard);

            // Assert
            Assert.IsTrue(result, "The Visa card number should be valid.");
        }

        [Test]
        public void ValidateMasterCardNumber_ShouldReturnTrue_ForValidMasterCard()
        {
            // Arrange
            string validMasterCard = "5555555555554444";

            // Act
            var result = _service.ValidateNumber(validMasterCard);

            // Assert
            Assert.IsTrue(result, "The MasterCard number should be valid.");
        }

        [Test]
        public void ValidateAmexCardNumber_ShouldReturnTrue_ForValidAmericanExpress()
        {
            // Arrange
            string validAmexCard = "378282246310005";

            // Act
            var result = _service.ValidateNumber(validAmexCard);

            // Assert
            Assert.IsTrue(result, "The American Express card number should be valid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnTrue_ForValidOwner()
        {
            // Arrange
            string validOwner = "John Doe";

            // Act
            var result = _service.ValidateOwner(validOwner);

            // Assert
            Assert.IsTrue(result, "Owner name should be valid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnFalse_ForInvalidOwner()
        {
            // Arrange
            string invalidOwner = "John123";  // Owner name contains numbers.

            // Act
            var result = _service.ValidateOwner(invalidOwner);

            // Assert
            Assert.IsFalse(result, "Owner name with numbers should be invalid.");
        }

        [Test]
        public void ValidateIssueDate_ShouldReturnTrue_ForFutureDate()
        {
            // Arrange
            string validIssueDate = "12/25";

            // Act
            var result = _service.ValidateIssueDate(validIssueDate);

            // Assert
            Assert.IsTrue(result, "The issue date should be valid if it’s in the future.");
        }

        [Test]
        public void ValidateIssueDate_ShouldReturnFalse_ForPastDate()
        {
            // Arrange
            string pastIssueDate = "01/20";

            // Act
            var result = _service.ValidateIssueDate(pastIssueDate);

            // Assert
            Assert.IsFalse(result, "The issue date should be invalid if it’s in the past.");
        }

        [Test]
        public void ValidateCvc_ShouldReturnTrue_ForValidThreeDigitCvc()
        {
            // Arrange
            string validCvc = "123";

            // Act
            var result = _service.ValidateCvc(validCvc);

            // Assert
            Assert.IsTrue(result, "Three-digit CVC should be valid.");
        }

        [Test]
        public void ValidateCvc_ShouldReturnFalse_ForInvalidCvc()
        {
            // Arrange
            string invalidCvc = "12";  // Less than 3 digits.

            // Act
            var result = _service.ValidateCvc(invalidCvc);

            // Assert
            Assert.IsFalse(result, "A CVC with fewer than 3 digits should be invalid.");
        }

        [Test]
        public void GetPaymentSystemType_ShouldReturnVisa_ForValidVisaCard()
        {
            // Arrange
            string visaCard = "4111111111111111";

            // Act
            var paymentSystem = _service.GetPaymentSystemType(visaCard);

            // Assert
            Assert.That(paymentSystem, Is.EqualTo(PaymentSystemType.Visa), "The payment system should be Visa.");
        }

        [Test]
        public void GetPaymentSystemType_ShouldThrowException_ForUnknownCardType()
        {
            // Arrange
            string unknownCard = "9999999999999999";  // Card type not supported.

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(unknownCard), "An unknown card type should throw a NotImplementedException.");
        }
    }
}
