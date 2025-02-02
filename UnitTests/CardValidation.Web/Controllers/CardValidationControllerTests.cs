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
            Assert.IsInstanceOf<BadRequestObjectResult>(result, "Should return BadRequest when model state is invalid.");
        }

        [Test]
        public void ValidateCreditCard_ShouldReturnOk_WhenCreditCardIsValid()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "12/25",
                Cvv = "123"
            };

            // Mock the service to return a Visa card type
            _cardValidationServiceMock
                .Setup(service => service.GetPaymentSystemType(creditCard.Number))
                .Returns(PaymentSystemType.Visa);

            // Act
            var result = _controller.ValidateCreditCard(creditCard) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result, "The result should be OkObjectResult.");
            Assert.That(result.StatusCode, Is.EqualTo(200), "The status code should be 200.");
            Assert.That(result.Value, Is.EqualTo(PaymentSystemType.Visa), "The returned payment system type should be Visa.");
        }

        [Test]
        public void ValidateCreditCard_ShouldHandleNullCardNumber()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "Jane Doe",
                Number = null,  // Null card number
                Date = "12/25",
                Cvv = "456"
            };

            // Mock the service to handle empty card number and return a NotImplementedException
            _cardValidationServiceMock
                .Setup(service => service.GetPaymentSystemType(string.Empty))
                .Throws(new NotImplementedException());

            // Act
            TestDelegate action = () => _controller.ValidateCreditCard(creditCard);

            // Assert
            Assert.Throws<NotImplementedException>(action, "Should throw NotImplementedException when card number is null.");
        }
    }
}
