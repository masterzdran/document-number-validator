namespace DocumentNumber.France.Vat.Generator.Tests
{
    using DocumentNumber.France.Vat.Generator;
    using Shouldly;
    using Xunit;

    public class VatGeneratorTests
    {
        private readonly int VAT_LENGTH = 13;

        [Theory(DisplayName = "CalculateCheckDigit returns expected check key for known SIREN numbers")]
        [InlineData(443061841, 64)]
        [InlineData(732829320, 44)]
        [InlineData(100000000, 61)]
        [InlineData(0, 12)]
        [InlineData(97, 12)]
        public void CalculateCheckDigitReturnsExpectedValue(long siren, int expectedCheckKey)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkKey = vatGenerator.CalculateCheckDigit(siren);

            // Assert
            checkKey.ShouldBe(expectedCheckKey);
        }

        [Fact(DisplayName = "CalculateCheckDigit should always return a value between 0 and 96")]
        public void CalculateCheckDigitShouldReturnValueInRange()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act & Assert
            for (long i = 0; i < 97; i++)
            {
                int checkKey = vatGenerator.CalculateCheckDigit(i);
                checkKey.ShouldBeGreaterThanOrEqualTo(0);
                checkKey.ShouldBeLessThanOrEqualTo(96);
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber should return a non-empty string")]
        public void GenerateDocumentNumberShouldReturnNonEmptyString()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber();

            // Assert
            generated.ShouldNotBeNullOrEmpty();
        }

        [Fact(DisplayName = "GenerateDocumentNumber should return a 13-character string")]
        public void GenerateDocumentNumberShouldReturnCorrectLength()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber();

            // Assert
            generated.Length.ShouldBe(VAT_LENGTH);
        }

        [Fact(DisplayName = "GenerateDocumentNumber should start with 'FR'")]
        public void GenerateDocumentNumberShouldStartWithFR()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber();

            // Assert
            generated.ShouldStartWith("FR");
        }

        [Fact(DisplayName = "GenerateDocumentNumber should have valid format FR + 2 digit key + 9 digit SIREN")]
        public void GenerateDocumentNumberShouldHaveValidFormat()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();

                // Assert
                generated.Length.ShouldBe(VAT_LENGTH);
                generated.ShouldStartWith("FR");
                generated.Substring(2, 2).ShouldMatch(@"^\d{2}$");
                generated.Substring(4, 9).ShouldMatch(@"^\d{9}$");
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber should have consistent check key for its SIREN")]
        public void GenerateDocumentNumberShouldHaveConsistentCheckKey()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();
                int checkKey = int.Parse(generated.Substring(2, 2));
                long siren = long.Parse(generated.Substring(4, 9));
                int expectedCheckKey = vatGenerator.CalculateCheckDigit(siren);

                // Assert
                checkKey.ShouldBe(expectedCheckKey);
            }
        }

        [Theory(DisplayName = "GenerateDocumentNumber with single-digit start should produce valid VAT")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        public void GenerateDocumentNumberWithSingleDigitStartShouldProduceValidVat(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(VAT_LENGTH);
            generated.ShouldStartWith("FR");
            long siren = long.Parse(generated.Substring(4, 9));
            siren.ToString().ShouldStartWith(startWith.ToString());
            int checkKey = int.Parse(generated.Substring(2, 2));
            int expectedCheckKey = vatGenerator.CalculateCheckDigit(siren);
            checkKey.ShouldBe(expectedCheckKey);
        }

        [Theory(DisplayName = "GenerateDocumentNumber with double-digit start should produce valid VAT")]
        [InlineData(10)]
        [InlineData(44)]
        [InlineData(99)]
        public void GenerateDocumentNumberWithDoubleDigitStartShouldProduceValidVat(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(VAT_LENGTH);
            generated.ShouldStartWith("FR");
            long siren = long.Parse(generated.Substring(4, 9));
            siren.ToString().ShouldStartWith(startWith.ToString());
            int checkKey = int.Parse(generated.Substring(2, 2));
            int expectedCheckKey = vatGenerator.CalculateCheckDigit(siren);
            checkKey.ShouldBe(expectedCheckKey);
        }

        [Theory(DisplayName = "GenerateDocumentNumber with triple-digit start should produce valid VAT")]
        [InlineData(100)]
        [InlineData(443)]
        [InlineData(999)]
        public void GenerateDocumentNumberWithTripleDigitStartShouldProduceValidVat(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(VAT_LENGTH);
            generated.ShouldStartWith("FR");
            long siren = long.Parse(generated.Substring(4, 9));
            siren.ToString().ShouldStartWith(startWith.ToString());
            int checkKey = int.Parse(generated.Substring(2, 2));
            int expectedCheckKey = vatGenerator.CalculateCheckDigit(siren);
            checkKey.ShouldBe(expectedCheckKey);
        }
    }
}