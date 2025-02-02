using CardValidation.Core.Services.Interfaces;
using CardValidation.Infrustructure;
using CardValidation.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace CardValidation.Tests.Infrustructure
{
    [TestFixture]
    public class CreditCardValidationFilterTests
    {
        private Mock<ICardValidationService> _mockCardValidationService;
        private CreditCardValidationFilter _filter;

        [SetUp]
        public void SetUp()
        {
            _mockCardValidationService = new Mock<ICardValidationService>();
            _filter = new CreditCardValidationFilter(_mockCardValidationService.Object);
        }

        private ActionExecutingContext CreateActionContext(CreditCard creditCard)
        {
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ControllerActionDescriptor());
            var actionArguments = new Dictionary<string, object> { { "creditCard", creditCard } };
            return new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), actionArguments, new object());
        }

        [Test]
        public void OnActionExecuting_ShouldAddModelError_WhenOwnerIsInvalid()
        {
            // Arrange
            var creditCard = new CreditCard { Owner = "123", Number = "4111111111111111", Date = "12/25", Cvv = "123" };
            _mockCardValidationService.Setup(service => service.ValidateOwner(It.IsAny<string>())).Returns(false);

            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsTrue(context.ModelState.ContainsKey("Owner"));
            Assert.That(context.ModelState["Owner"].Errors[0].ErrorMessage, Is.EqualTo("Wrong owner"));
        }

        [Test]
        public void OnActionExecuting_ShouldAddModelError_WhenCvcIsInvalid()
        {
            // Arrange
            var creditCard = new CreditCard { Owner = "John Doe", Number = "4111111111111111", Date = "12/25", Cvv = "12" };
            _mockCardValidationService.Setup(service => service.ValidateCvc(It.IsAny<string>())).Returns(false);

            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsTrue(context.ModelState.ContainsKey("Cvv"));
            Assert.That(context.ModelState["Cvv"].Errors[0].ErrorMessage, Is.EqualTo("Wrong cvv"));
        }

        [Test]
        public void OnActionExecuting_ShouldAddModelError_WhenNumberIsMissing()
        {
            // Arrange
            var creditCard = new CreditCard { Owner = "John Doe", Date = "12/25", Cvv = "123" };
            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsTrue(context.ModelState.ContainsKey("Number"));
            Assert.That(context.ModelState["Number"].Errors[0].ErrorMessage, Is.EqualTo("Number is required"));
        }

        [Test]
        public void OnActionExecuting_ShouldAddModelError_WhenIssueDateIsInvalid()
        {
            // Arrange
            var creditCard = new CreditCard { Owner = "John Doe", Number = "4111111111111111", Date = "13/25", Cvv = "123" };
            _mockCardValidationService.Setup(service => service.ValidateIssueDate(It.IsAny<string>())).Returns(false);

            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsTrue(context.ModelState.ContainsKey("Date"));
            Assert.That(context.ModelState["Date"].Errors[0].ErrorMessage, Is.EqualTo("Wrong date"));
        }

        [Test]
        public void OnActionExecuting_ShouldNotAddModelErrors_WhenAllFieldsAreValid()
        {
            // Arrange
            var creditCard = new CreditCard { Owner = "John Doe", Number = "4111111111111111", Date = "12/25", Cvv = "123" };
            _mockCardValidationService.Setup(service => service.ValidateOwner(It.IsAny<string>())).Returns(true);
            _mockCardValidationService.Setup(service => service.ValidateNumber(It.IsAny<string>())).Returns(true);
            _mockCardValidationService.Setup(service => service.ValidateIssueDate(It.IsAny<string>())).Returns(true);
            _mockCardValidationService.Setup(service => service.ValidateCvc(It.IsAny<string>())).Returns(true);

            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsEmpty(context.ModelState, "ModelState should be empty when all fields are valid.");
        }

        [Test]
        public void OnActionExecuting_ShouldThrowException_WhenCreditCardIsNull()
        {
            // Arrange
            var context = CreateActionContext(null);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _filter.OnActionExecuting(context), "An exception should be thrown when the credit card is null.");
        }
    }
}
