namespace DocumentNumber.Portugal.Nif.Generator.Tests
{
    using DocumentNumber.Portugal.Vat.Generator;
    using DocumentNumber.Portugal.Vat.Validator;
    using Shouldly;
    using Xunit;

    public class VatGeneratorTests
    {
        private readonly int NIF_LENGHT = 9;

        [Fact(DisplayName = "For given unchecked nif '19214394', the return checkdigit must be '8'")]
        public void Test1()
        {
            // Arrange
            long uncheckedNif = 19214394;
            int resultCheckDigit = 8;
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkDigit = vatGenerator.CalculateCheckDigit(uncheckedNif);

            // Assert
            checkDigit.ShouldBe(resultCheckDigit);
        }

        [Fact(DisplayName = "Random Generated Nif should have 9 characters long.")]
        public void GeneratedNifShouldHaveNineDigits()
        {
            // Arrange
            IVatGenerator nifGenerator = new VatGenerator();

            // Act
            string generatedNif = nifGenerator.GenerateDocumentNumber();

            // Assert
            generatedNif.ShouldNotBeNullOrEmpty();
            generatedNif.Length.ShouldBe(NIF_LENGHT);
        }

        [Theory(DisplayName = "For Given input, Generated Nifs should be valid.")]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(45)]
        [InlineData(70)]
        [InlineData(71)]
        [InlineData(72)]
        [InlineData(77)]
        [InlineData(78)]
        [InlineData(79)]
        [InlineData(90)]
        [InlineData(98)]
        [InlineData(99)]
        public void GivenNumberGeneratedNifShouldHaveNineDigits(int expected)
        {
            // Arrange
            IVatGenerator nifGenerator = new VatGenerator();
            IVatValidator nifValidator = new VatValidator();

            // Act
            string generatedNif = nifGenerator.GenerateDocumentNumber(expected);
            bool isValid = nifValidator.Validate(generatedNif);

            // Assert
            generatedNif.ShouldNotBeNullOrEmpty();
            isValid.ShouldBeTrue();
        }
    }
}