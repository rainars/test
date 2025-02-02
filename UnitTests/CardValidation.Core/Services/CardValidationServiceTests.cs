using CardValidation.Core.Services;
using NUnit.Framework;

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
            Assert.IsFalse(result, $"The owner name '{owner}' should be considered invalid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnFalse_WhenOwnerIsNull()
        {
            // Arrange
            string? owner = null; // Nullable reference type.

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsFalse(result, "Null owner name should be considered invalid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnFalse_WhenOwnerExceedsMaxLength()
        {
            // Arrange
            var owner = new string('A', 101); // Assuming max length is 100 characters.

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsFalse(result, "Owner name exceeding max length should be considered invalid.");
        }

        [Test]
        public void ValidateOwner_ShouldReturnTrue_WhenOwnerIsExactlyMaxLength()
        {
            // Arrange
            var owner = new string('A', 100); // Assuming max length is 100 characters.

            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsTrue(result, "Owner name of exactly max length should be considered valid.");
        }

        [TestCase("john doe", Description = "Lowercase should be allowed.")]
        [TestCase("JOHN DOE", Description = "Uppercase should be allowed.")]
        [TestCase("John A. Doe", Description = "Names with middle initial should be allowed.")]
        [TestCase("John Mary Lee", Description = "Names with multiple words should be allowed.")]
        public void ValidateOwner_ShouldReturnTrue_ForValidOwnerNames(string owner)
        {
            // Act
            var result = _service.ValidateOwner(owner);

            // Assert
            Assert.IsTrue(result, $"The owner name '{owner}' should be considered valid.");
        }
    }
}
