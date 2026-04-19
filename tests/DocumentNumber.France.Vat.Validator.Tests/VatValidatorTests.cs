namespace DocumentNumber.France.Vat.Validator.Tests
{
    using DocumentNumber.France.Vat.Generator;
    using DocumentNumber.France.Vat.Validator;
    using Shouldly;
    using Xunit;

    public sealed class VatValidatorTests
    {
        [Theory(DisplayName = "Valid French VAT should return true.")]
        [InlineData("FR44732829320")]
        public void Vat_ShouldBe_Valid(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Null value should return false.")]
        [InlineData(null)]
        public void Vat_ShouldBe_Invalid_If_Value_Is_Null(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Invalid length should return false.")]
        [InlineData("FR4473282932")]
        [InlineData("FR447328293200")]
        [InlineData("FR44")]
        [InlineData("")]
        public void Vat_ShouldBe_Invalid_If_Length_Is_Not_Thirteen(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Value not starting with FR should return false.")]
        [InlineData("DE44732829320")]
        [InlineData("ES44732829320")]
        [InlineData("1244732829320")]
        public void Vat_ShouldBe_Invalid_If_Not_Starting_With_FR(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Non-numeric check key should return false.")]
        [InlineData("FRAB732829320")]
        [InlineData("FR.4732829320")]
        public void Vat_ShouldBe_Invalid_If_CheckKey_Is_Not_Numeric(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Non-numeric SIREN should return false.")]
        [InlineData("FR4473282932A")]
        [InlineData("FR44ABCDEFGHI")]
        public void Vat_ShouldBe_Invalid_If_Siren_Is_Not_Numeric(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Wrong check key should return false.")]
        [InlineData("FR00732829320")]
        [InlineData("FR99732829320")]
        public void Vat_ShouldBe_Invalid_If_CheckKey_Is_Wrong(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Value with leading/trailing spaces should still validate correctly.")]
        [InlineData(" FR44732829320 ")]
        public void Vat_ShouldBe_Valid_After_Trimming(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Value with only whitespace should return false.")]
        [InlineData("             ")]
        [InlineData("  ")]
        public void Vat_ShouldBe_Invalid_If_Value_Is_Whitespace(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "Generated French VAT numbers should all be validated as valid.")]
        public void Generated_Vat_ShouldBe_Valid()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = generator.GenerateDocumentNumber();
                bool validationResult = validator.Validate(generated);

                // Assert
                validationResult.ShouldBeTrue();
            }
        }

        [Theory(DisplayName = "Generated French VAT with specific start should be valid.")]
        [InlineData(1)]
        [InlineData(44)]
        [InlineData(443)]
        public void Generated_Vat_With_Start_ShouldBe_Valid(int startWith)
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            // Act
            string generated = generator.GenerateDocumentNumber(startWith);
            bool validationResult = validator.Validate(generated);

            // Assert
            validationResult.ShouldBeTrue();
        }
    }
}
