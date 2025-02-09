﻿using CardValidation.Core.Services.Interfaces;
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
            var actionArguments = new Dictionary<string, object?> { { "creditCard", creditCard } }; // Ensure compatibility with IDictionary<string, object?>
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
            Assert.That(context.ModelState, Is.Not.Null, "ModelState should not be null");
            Assert.That(context.ModelState.ContainsKey("Owner"), "ModelState should contain key 'Owner'");

            var ownerErrors = context.ModelState["Owner"]?.Errors;
            Assert.That(ownerErrors, Is.Not.Null, "Errors for 'Owner' should not be null");
            Assert.That(ownerErrors.Count > 0, "There should be at least one error for 'Owner'");

            Assert.That(ownerErrors[0].ErrorMessage, Is.EqualTo("Wrong owner"));
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
            Assert.IsNotNull(context.ModelState, "ModelState should not be null");
            Assert.IsTrue(context.ModelState.ContainsKey("Cvv"), "ModelState should contain key 'Cvv'");

            var cvcErrors = context.ModelState["Cvv"]?.Errors;
            Assert.IsNotNull(cvcErrors, "Errors for 'Cvv' should not be null");
            Assert.IsTrue(cvcErrors.Count > 0, "There should be at least one error for 'Cvv'");

            Assert.That(cvcErrors[0].ErrorMessage, Is.EqualTo("Wrong cvv"));
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
            Assert.That(context.ModelState, Is.Not.Null, "ModelState should not be null");
            Assert.IsTrue(context.ModelState.ContainsKey("Number"), "ModelState should contain key 'Number'");

            var numberErrors = context.ModelState["Number"]?.Errors;
            Assert.IsNotNull(numberErrors, "Errors for 'Number' should not be null");
            Assert.IsTrue(numberErrors.Count > 0, "There should be at least one error for 'Number'");

            Assert.That(numberErrors[0].ErrorMessage, Is.EqualTo("Number is required"));
        }


        [Test]
        public void OnActionExecuting_ShouldAddModelError_WhenIssueDateIsInvalid()
        {
            // Arrange
            var creditCard = new CreditCard
            {
                Owner = "John Doe",
                Number = "4111111111111111",
                Date = "13/25",
                Cvv = "123"
            };
            _mockCardValidationService.Setup(service => service.ValidateIssueDate(It.IsAny<string>())).Returns(false);

            var context = CreateActionContext(creditCard);

            // Act
            _filter.OnActionExecuting(context);

            // Assert
            Assert.IsNotNull(context.ModelState, "ModelState should not be null");
            Assert.IsTrue(context.ModelState.ContainsKey("Date"), "ModelState should contain key 'Date'");

            var dateErrors = context.ModelState["Date"]?.Errors;
            Assert.IsNotNull(dateErrors, "Errors for 'Date' should not be null");
            Assert.IsTrue(dateErrors.Count > 0, "There should be at least one error for 'Date'");

            Assert.That(dateErrors[0].ErrorMessage, Is.EqualTo("Wrong date"));
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
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new ControllerActionDescriptor());
            var actionArguments = new Dictionary<string, object?> { { "creditCard", null } }; // Explicitly set to null
            var context = new ActionExecutingContext(actionContext, new List<IFilterMetadata>(), actionArguments, new object());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _filter.OnActionExecuting(context), "An exception should be thrown when the credit card is null.");
        }

    }
}
