namespace ValidationTests
{
    using DocumentNumber.Portugal.Vat.Validator;
    using Shouldly;
    using Xunit;

    public sealed class VatValidatorTests
    {
        [Theory(DisplayName = "Valid NIF with 9 digit characters long.")]
        [InlineData("287487040")]
        public void Nif_ShouldBe_Valid_If_Have_Nine_Characters_Digits_Length(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Null NIF should return false.")]
        [InlineData(null)]
        public void Nif_ShouldBe_InValid_If_Value_Is_NULL(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Invalid NIF with 9 digit characters long.")]
        [InlineData("123456780")]
        public void Nif_ShouldBe_InValid_If_Has_Nine_Characters_Digits_Length_And_Is_Invalid(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Invalid NIF without 9 digit characters long.")]
        [InlineData("1234567891011")]
        [InlineData("12345678")]
        [InlineData("1234567")]
        [InlineData("123456")]
        public void Nif_ShouldBe_InValid_If_Doesnt_Have_Nine_Characters_Digits_Length(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Invalid NIF with letters.")]
        [InlineData("N12345678")]
        [InlineData("12345678N")]
        [InlineData("N123M45678N")]
        [InlineData("N3M48N")]
        public void Nif_ShouldBe_InValid_If_Has_Letters_Characters_In_Value(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "InValid Nif for a person should be false.")]
        [InlineData("158837530")]
        [InlineData("253148500")]
        [InlineData("390716870")]
        [InlineData("452736901")]
        public void Nif_ShouldBe_InValid_For_A_Person(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "InValid Nif for a colective person should be false.")]
        [InlineData("588641050")]
        [InlineData("685341300")]
        [InlineData("881783320")]
        [InlineData("929115560")]
        public void Nif_ShouldBe_InValid_For_A_Collective_Person(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Other InValid Nif scopes should be false.")]
        [InlineData("707928130")]
        [InlineData("712747420")]
        [InlineData("727718040")]
        [InlineData("747200970")]
        [InlineData("750707951")]
        [InlineData("774349940")]
        [InlineData("781453890")]
        [InlineData("799438970")]
        [InlineData("805505860")]
        [InlineData("904549940")]
        [InlineData("929115560")]
        [InlineData("917227570")]
        [InlineData("985942950")]
        [InlineData("995063790")]
        public void Nif_ShouldBe_InValid_For_Others_Valid_Scope(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Valid Nif for a person")]
        [InlineData("158837533")]
        [InlineData("253148502")]
        [InlineData("390716871")]
        [InlineData("452736900")]
        public void Nif_ShouldBe_Valid_If_It_Is_For_A_Person(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Valid Nif for a colective person.")]
        [InlineData("588641057")]
        [InlineData("685341305")]
        [InlineData("881783323")]
        public void Nif_ShouldBe_Valid_If_It_Is_For_A_Collective_Person(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Other Valid Nif scopes.")]
        [InlineData("707928133")]
        [InlineData("712747427")]
        [InlineData("727718045")]
        [InlineData("747200971")]
        [InlineData("750707950")]
        [InlineData("774349948")]
        [InlineData("781453895")]
        [InlineData("799438979")]
        [InlineData("805505865")]
        [InlineData("904549941")]
        [InlineData("917227573")]
        [InlineData("985942959")]
        [InlineData("995063796")]
        public void Nif_ShouldBe_Valid_For_Others_Valid_Scope(string nif)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(nif);

            // Assert
            validationResult.ShouldBeTrue();
        }
    }
}