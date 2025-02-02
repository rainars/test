using CardValidation.Core.Enums;
using CardValidation.Core.Services.Interfaces;
using CardValidation.Core.Services;

[TestFixture]
public class CardValidationServiceTests
{
    private ICardValidationService _service;

    [SetUp]
    public void Setup()
    {
        _service = new CardValidationService();
    }

    [Test]
    public void GetPaymentSystemType_ShouldReturnVisa_ForValidVisaCard()
    {
        // Arrange
        string visaCard = "4111111111111111";

        // Act
        var paymentSystemType = _service.GetPaymentSystemType(visaCard);

        // Assert
        Assert.That(paymentSystemType, Is.EqualTo(PaymentSystemType.Visa), "Payment system type should be Visa.");
    }

    [Test]
    public void GetPaymentSystemType_ShouldReturnMasterCard_ForValidMasterCard()
    {
        // Arrange
        string masterCard = "5555555555554444";

        // Act
        var paymentSystemType = _service.GetPaymentSystemType(masterCard);

        // Assert
        Assert.That(paymentSystemType, Is.EqualTo(PaymentSystemType.MasterCard), "Payment system type should be MasterCard.");
    }

    [Test]
    public void GetPaymentSystemType_ShouldReturnAmex_ForValidAmexCard()
    {
        // Arrange
        string amexCard = "378282246310005";

        // Act
        var paymentSystemType = _service.GetPaymentSystemType(amexCard);

        // Assert
        Assert.That(paymentSystemType, Is.EqualTo(PaymentSystemType.AmericanExpress), "Payment system type should be American Express.");
    }

    [Test]
    public void GetPaymentSystemType_ShouldThrowException_ForUnknownCardType()
    {
        // Arrange
        string unknownCard = "9999999999999999";  // Invalid card number.

        // Act & Assert
        Assert.Throws<NotImplementedException>(() => _service.GetPaymentSystemType(unknownCard), "Unknown card type should throw a NotImplementedException.");
    }
}
