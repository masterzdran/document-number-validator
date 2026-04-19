namespace DocumentNumber.Brazil.Vat.Validator.Tests
{
    using DocumentNumber.Brazil.Vat.Generator;
    using DocumentNumber.Brazil.Vat.Validator;
    using Shouldly;
    using Xunit;

    public sealed class VatValidatorTests
    {
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
        [InlineData("1234567890")]
        [InlineData("123456789012")]
        [InlineData("12345")]
        [InlineData("")]
        [InlineData("123456789012345")]
        public void Vat_ShouldBe_Invalid_If_Length_Is_Not_Eleven_Or_Fourteen(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Non-numeric value should return false.")]
        [InlineData("1234567890A")]
        [InlineData("ABCDEFGHIJK")]
        [InlineData("123.456.789")]
        public void Vat_ShouldBe_Invalid_If_Value_Is_Not_Numeric(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Repeated digits should return false.")]
        [InlineData("00000000000")]
        [InlineData("11111111111")]
        [InlineData("22222222222")]
        [InlineData("33333333333")]
        [InlineData("44444444444")]
        [InlineData("55555555555")]
        [InlineData("66666666666")]
        [InlineData("77777777777")]
        [InlineData("88888888888")]
        [InlineData("99999999999")]
        public void Vat_ShouldBe_Invalid_If_All_Digits_Are_Same(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "CPF with wrong first check digit should return false.")]
        [InlineData("12345678901")]
        public void Vat_ShouldBe_Invalid_If_First_CheckDigit_Is_Wrong(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "CPF with wrong second check digit should return false.")]
        public void Vat_ShouldBe_Invalid_If_Second_CheckDigit_Is_Wrong()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            string generated = generator.GenerateDocumentNumber(1);
            // Flip last digit to make second check digit wrong
            int lastDigit = generated[10] - '0';
            int wrongDigit = (lastDigit + 1) % 10;
            string invalid = generated.Substring(0, 10) + wrongDigit.ToString();

            // Act
            bool validationResult = validator.Validate(invalid);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Value with only whitespace should return false.")]
        [InlineData("           ")]
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

        [Fact(DisplayName = "Generated CPF should be validated as valid.")]
        public void Generated_Cpf_ShouldBe_Valid()
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

        [Theory(DisplayName = "Generated CPF with specific start should be valid.")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(10)]
        [InlineData(99)]
        [InlineData(100)]
        public void Generated_Cpf_With_Start_ShouldBe_Valid(int startWith)
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

        [Theory(DisplayName = "Value with leading/trailing spaces should still validate correctly.")]
        [InlineData(1)]
        [InlineData(5)]
        public void Vat_ShouldBe_Valid_After_Trimming(int startWith)
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();
            string generated = generator.GenerateDocumentNumber(startWith);
            string padded = $" {generated} ";

            // Act
            bool validationResult = validator.Validate(padded);

            // Assert
            validationResult.ShouldBeTrue();
        }

        [Fact(DisplayName = "Generated CNPJ should be validated as valid.")]
        public void Generated_Cnpj_ShouldBe_Valid()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = generator.GenerateCnpjNumber();
                bool validationResult = validator.Validate(generated);

                // Assert
                validationResult.ShouldBeTrue();
            }
        }

        [Fact(DisplayName = "CNPJ with wrong first check digit should return false.")]
        public void Cnpj_ShouldBe_Invalid_If_First_CheckDigit_Is_Wrong()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            string generated = generator.GenerateCnpjNumber();
            int digit12 = generated[12] - '0';
            int wrongDigit = (digit12 + 1) % 10;
            string invalid = generated.Substring(0, 12) + wrongDigit.ToString() + generated[13];

            // Act
            bool validationResult = validator.Validate(invalid);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "CNPJ with wrong second check digit should return false.")]
        public void Cnpj_ShouldBe_Invalid_If_Second_CheckDigit_Is_Wrong()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();

            string generated = generator.GenerateCnpjNumber();
            int lastDigit = generated[13] - '0';
            int wrongDigit = (lastDigit + 1) % 10;
            string invalid = generated.Substring(0, 13) + wrongDigit.ToString();

            // Act
            bool validationResult = validator.Validate(invalid);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Repeated 14-digit values should return false.")]
        [InlineData("00000000000000")]
        [InlineData("11111111111111")]
        [InlineData("99999999999999")]
        public void Cnpj_ShouldBe_Invalid_If_All_Digits_Are_Same(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Theory(DisplayName = "Non-numeric 14-character value should return false.")]
        [InlineData("1234567890123A")]
        [InlineData("ABCDEFGHIJKLMN")]
        public void Cnpj_ShouldBe_Invalid_If_Not_Numeric(string vat)
        {
            //Arrange
            var validator = new VatValidator();

            // Act
            bool validationResult = validator.Validate(vat);

            // Assert
            validationResult.ShouldBeFalse();
        }

        [Fact(DisplayName = "CNPJ with leading/trailing spaces should still validate.")]
        public void Cnpj_ShouldBe_Valid_After_Trimming()
        {
            //Arrange
            var generator = new VatGenerator();
            var validator = new VatValidator();
            string generated = generator.GenerateCnpjNumber();
            string padded = $" {generated} ";

            // Act
            bool validationResult = validator.Validate(padded);

            // Assert
            validationResult.ShouldBeTrue();
        }
    }
}