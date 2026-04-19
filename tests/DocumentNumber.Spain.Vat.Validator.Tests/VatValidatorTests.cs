namespace DocumentNumber.Spain.Vat.Validator.Tests
{
    using DocumentNumber.Spain.Vat.Generator;
    using DocumentNumber.Spain.Vat.Validator;
    using Shouldly;
    using Xunit;

    public sealed class VatValidatorTests
    {
        [Theory(DisplayName = "Valid DNI should return true.")]
        [InlineData("12345678Z")]
        [InlineData("00000000T")]
        [InlineData("99999999R")]
        public void Dni_ShouldBe_Valid(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Valid NIE starting with X should return true.")]
        [InlineData("X0000000T")]
        public void Nie_StartingWithX_ShouldBe_Valid(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Fact(DisplayName = "Valid NIE starting with Y should return true.")]
        public void Nie_StartingWithY_ShouldBe_Valid()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();
            long fullNumber = 1 * 10000000L + 0;
            char checkDigit = generator.CalculateCheckDigit(fullNumber);
            string vat = $"Y0000000{checkDigit}";

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Theory(DisplayName = "Valid NIE starting with Z should return true.")]
        [InlineData("Z0000000M")]
        public void Nie_StartingWithZ_ShouldBe_Valid(string vat)
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
        [InlineData("12345678")]
        [InlineData("1234567890")]
        [InlineData("1234")]
        [InlineData("")]
        public void Vat_ShouldBe_Invalid_If_Length_Is_Not_Nine(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Value with only whitespace should return false.")]
        [InlineData("         ")]
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

        [Theory(DisplayName = "DNI with wrong check digit should return false.")]
        [InlineData("12345678A")]
        [InlineData("00000000A")]
        public void Dni_ShouldBe_Invalid_If_CheckDigit_Is_Wrong(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "NIE with wrong check digit should return false.")]
        [InlineData("X0000000A")]
        [InlineData("Y0000000A")]
        [InlineData("Z0000000A")]
        public void Nie_ShouldBe_Invalid_If_CheckDigit_Is_Wrong(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Invalid first character should return false.")]
        [InlineData("I12345678")]
        [InlineData("K12345678")]
        [InlineData("L12345678")]
        public void Vat_ShouldBe_Invalid_If_FirstChar_Is_Not_Digit_Or_XYZ_Or_Cif(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "DNI with non-digit characters in number part should return false.")]
        [InlineData("1234A678Z")]
        public void Dni_ShouldBe_Invalid_If_Number_Part_Contains_Letters(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "NIE with non-digit characters in number part should return false.")]
        [InlineData("X000A000T")]
        public void Nie_ShouldBe_Invalid_If_Number_Part_Contains_Letters(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "Generated DNI should be validated as valid.")]
        public void Generated_Dni_ShouldBe_Valid()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = generator.GenerateDocumentNumber(1);
                bool validationResult = validator.Validate(generated);

                // Assert
                validationResult.ShouldBeTrue();
            }
        }

        [Fact(DisplayName = "Generated document numbers should all be validated as valid.")]
        public void Generated_DocumentNumber_ShouldBe_Valid()
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

        [Theory(DisplayName = "Value with leading/trailing spaces should still validate correctly.")]
        [InlineData(" 12345678Z ")]
        public void Vat_ShouldBe_Valid_After_Trimming(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Fact(DisplayName = "Generated CIF should be validated as valid.")]
        public void Generated_Cif_ShouldBe_Valid()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            for (int i = 0; i < 100; i++)
            {
                // Act
                string generated = generator.GenerateCifNumber();
                bool validationResult = validator.Validate(generated);

                // Assert
                validationResult.ShouldBeTrue();
            }
        }

        [Theory(DisplayName = "CIF with wrong control character should return false.")]
        [InlineData("A12345670")]
        public void Cif_ShouldBe_Invalid_If_Control_Is_Wrong(string vat)
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            // First verify what the correct control is, then ensure our test value is wrong
            char correctControl = generator.CalculateCifControl("1234567", 'A');
            if (vat[8] == correctControl)
            {
                // If by chance the test value is correct, skip
                return;
            }

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "CIF with non-digit middle part should return false.")]
        [InlineData("A12AB5670")]
        public void Cif_ShouldBe_Invalid_If_Middle_Contains_Letters(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "CIF with leading/trailing spaces should still validate.")]
        public void Cif_ShouldBe_Valid_After_Trimming()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();
            string generated = generator.GenerateCifNumber();
            string padded = $" {generated} ";

            // Act
            bool validationResult = validator.Validate(padded);

            // Assert
            validationResult.ShouldBeTrue();
        }
    }
}
