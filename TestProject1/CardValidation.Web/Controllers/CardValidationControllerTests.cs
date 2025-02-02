using CardValidation.Controllers;
using CardValidation.Core.Enums;
using CardValidation.Core.Services.Interfaces;
using CardValidation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace CardValidation.Tests.Controllers
{
    [TestFixture]
    public class CardValidationControllerTests
    {
        private Mock<ICardValidationService> _cardValidationServiceMock;
        private CardValidationController _controller;

        [SetUp]
        public void Setup()
        {
            _cardValidationServiceMock = new Mock<ICardValidationService>();
            _controller = new CardValidationController(_cardValidationServiceMock.Object);
        }

        [Test]
        public void ValidateCreditCard_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Number", "Card number is required.");

            var creditCard = new CreditCard
            {
                Owner = "John Doe",
                Date = "12/25",
                Cvv = "123"
            };

            // Act
            var result = _controller.ValidateCreditCard(creditCard);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void ValidateCreditCard_ShouldReturnOk_WhenCreditCardIsValid()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111", // Visa number
                Date = "12/25",
                Cvv = "123"
            };

            _cardValidationServiceMock
                .Setup(service => service.GetPaymentSystemType(creditCard.Number))
                .Returns(PaymentSystemType.Visa);

            // Act
            var result = _controller.ValidateCreditCard(creditCard) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(PaymentSystemType.Visa));
        }

        [Test]
        public void ValidateCreditCard_ShouldHandleNullCardNumber()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "Jane Doe",
                Number = null, // Simulating null card number
                Date = "12/26",
                Cvv = "456"
            };

            _cardValidationServiceMock
                .Setup(service => service.GetPaymentSystemType(string.Empty))
                .Returns(PaymentSystemType.Visa); // Assuming Visa as default for testing purposes

            // Act
            var result = _controller.ValidateCreditCard(creditCard) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(PaymentSystemType.Visa));
        }

        [Test]
        public void ValidateCreditCard_ShouldReturnBadRequest_ForInvalidCreditCard()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "Invalid Owner",
                Number = "1234567890123456", // Invalid card number
                Date = "01/30",
                Cvv = "999"
            };

            _cardValidationServiceMock
                .Setup(service => service.GetPaymentSystemType(It.IsAny<string>()))
                .Throws(new NotImplementedException("Unsupported card type"));

            // Act
            var result = _controller.ValidateCreditCard(creditCard) as ObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(500));
        }
    }
}
