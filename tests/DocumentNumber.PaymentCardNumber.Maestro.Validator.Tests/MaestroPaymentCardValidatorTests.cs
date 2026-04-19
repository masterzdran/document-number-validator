using DocumentNumber.PaymentCardNumber.Common.Enums;
using DocumentNumber.PaymentCardNumber.Common.PaymentCard;
using DocumentNumber.ValidatorAbstractions;
using Shouldly;
using Xunit;

namespace DocumentNumber.PaymentCardNumber.Maestro.Validator.Tests
{
    public sealed class MaestroPaymentCardValidatorTests
    {
        [Fact(DisplayName = "PaymentCard Issuer should be Maestro.")]
        public void PaymentCard_IssuerIdentity_Should_is_Valid()
        {
            // Arrange
            IPaymentCardDocumentValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.IssuerIdentity;

            // Assert
            result.ShouldBe(PaymentCardIssuer.Maestro);
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid For Null Input.")]
        [InlineData(null)]
        public void PaymentCardNumber_Is_Invalid_If_Input_Is_Null(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid For Empty Input.")]
        [InlineData("")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Is_Empty(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid For Spaces Input.")]
        [InlineData("  ")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Is_Spaces(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid if Input has Length not equal to 12 to 19.")]
        [InlineData("012345678901")]
        [InlineData("01234567890123")]
        [InlineData("012345678901234")]
        [InlineData("01234567890123456")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Has_Length_Not_Equal_to_12_to_19(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid if Input has non numeric characters.")]
        [InlineData("01234567890123A")]
        [InlineData("A012345678901234")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Has_Non_Numeric_Character(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid if Input has invalid Issuer identification number (INN) number.")]
        [InlineData("01234567890123")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Has_invalid_INN_number(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Invalid if Input has 12 to 19 characters Length invalid number.")]
        [InlineData("575300832170")]
        [InlineData("5753008321701")]
        [InlineData("57530083217012")]
        [InlineData("575300832170123")]
        [InlineData("5753008321701234")]
        [InlineData("57530083217012345")]
        [InlineData("575300832170123456")]
        [InlineData("5753008321701234567")]
        public void PaymentCardNumber_Is_Invalid_If_Input_Has_12_to_19_characters_length_invalid_number(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeFalse();
        }

        [Theory(DisplayName = "Payment Card Number Should be Valid if Input for 5018, 5020, 5038, 5893, 6304, 6759, 6761, 6762, 6763	codes.")]
        [InlineData("5018789696907010")]
        [InlineData("5020678165385780")]
        [InlineData("5038935643045877")]
        [InlineData("5893128092553542")]
        [InlineData("6304666385177090")]
        [InlineData("6759494081674983")]
        [InlineData("6761979903129808")]
        [InlineData("6762821797975756")]
        [InlineData("6763057893474266")]
        public void PaymentCardNumber_Is_Valid_If_Input_for_valid_codes(string value)
        {
            // Arrange
            IDocumentNumberValidator paymentCardValidator = new MaestroPaymentCardValidator();

            // Act
            var result = paymentCardValidator.Validate(value);

            // Assert
            result.ShouldBeTrue();
        }
    }
}