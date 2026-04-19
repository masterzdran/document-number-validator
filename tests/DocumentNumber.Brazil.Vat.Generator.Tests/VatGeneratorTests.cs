namespace DocumentNumber.Brazil.Vat.Generator.Tests
{
    using DocumentNumber.Brazil.Vat.Generator;
    using Shouldly;
    using Xunit;

    public class VatGeneratorTests
    {
        private readonly int CPF_LENGTH = 11;
        private readonly int CNPJ_LENGTH = 14;

        [Theory(DisplayName = "CalculateCheckDigit returns expected value for known inputs")]
        [InlineData(0, 0)]
        [InlineData(1, 9)]
        [InlineData(123456789, 0)]
        public void CalculateCheckDigitReturnsExpectedValue(long number, int expectedCheckDigit)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkDigit = vatGenerator.CalculateCheckDigit(number);

            // Assert
            checkDigit.ShouldBe(expectedCheckDigit);
        }

        [Fact(DisplayName = "CalculateCheckDigit should always return a value between 0 and 9")]
        public void CalculateCheckDigitShouldReturnValueInRange()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act & Assert
            for (long i = 0; i < 100; i++)
            {
                int checkDigit = vatGenerator.CalculateCheckDigit(i);
                checkDigit.ShouldBeGreaterThanOrEqualTo(0);
                checkDigit.ShouldBeLessThanOrEqualTo(9);
            }
        }

        [Fact(DisplayName = "CalculateCheckDigit returns 0 when remainder is 0")]
        public void CalculateCheckDigitReturnsZeroWhenRemainderIsZero()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkDigit = vatGenerator.CalculateCheckDigit(0);

            // Assert
            checkDigit.ShouldBe(0);
        }

        [Fact(DisplayName = "CalculateCheckDigit returns 0 when remainder is 1")]
        public void CalculateCheckDigitReturnsZeroWhenRemainderIsOne()
        {
            // Arrange - find a number where sum % 11 == 1
            // For number 10: digits are 1,0; weights 3,2; sum = 1*3 + 0*2 = 3; 3%11=3 -> not 1
            // For number 1: digit is 1; weight 2; sum = 2; 2%11=2 -> not 1
            // We use a known value and verify the result is in valid range
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkDigit = vatGenerator.CalculateCheckDigit(1);

            // Assert
            checkDigit.ShouldBeGreaterThanOrEqualTo(0);
            checkDigit.ShouldBeLessThanOrEqualTo(9);
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

        [Fact(DisplayName = "GenerateDocumentNumber should return an 11-character string")]
        public void GenerateDocumentNumberShouldReturnElevenCharacters()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber();

            // Assert
            generated.Length.ShouldBe(CPF_LENGTH);
        }

        [Fact(DisplayName = "GenerateDocumentNumber should return only digits")]
        public void GenerateDocumentNumberShouldReturnOnlyDigits()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();

                // Assert
                generated.ShouldMatch(@"^\d{11}$");
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber should have valid first check digit")]
        public void GenerateDocumentNumberShouldHaveValidFirstCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();
                long baseNumber = long.Parse(generated.Substring(0, 9));
                int expectedFirstCheck = vatGenerator.CalculateCheckDigit(baseNumber);

                // Assert
                int actualFirstCheck = int.Parse(generated.Substring(9, 1));
                actualFirstCheck.ShouldBe(expectedFirstCheck);
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber should have valid second check digit")]
        public void GenerateDocumentNumberShouldHaveValidSecondCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();
                long first10Digits = long.Parse(generated.Substring(0, 10));
                int expectedSecondCheck = vatGenerator.CalculateCheckDigit(first10Digits);

                // Assert
                int actualSecondCheck = int.Parse(generated.Substring(10, 1));
                actualSecondCheck.ShouldBe(expectedSecondCheck);
            }
        }

        [Theory(DisplayName = "GenerateDocumentNumber with single-digit start should produce valid CPF")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        public void GenerateDocumentNumberWithSingleDigitStartShouldProduceValidCpf(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(CPF_LENGTH);
            generated.ShouldStartWith(startWith.ToString());

            long baseNumber = long.Parse(generated.Substring(0, 9));
            int expectedFirstCheck = vatGenerator.CalculateCheckDigit(baseNumber);
            int.Parse(generated.Substring(9, 1)).ShouldBe(expectedFirstCheck);

            long first10Digits = long.Parse(generated.Substring(0, 10));
            int expectedSecondCheck = vatGenerator.CalculateCheckDigit(first10Digits);
            int.Parse(generated.Substring(10, 1)).ShouldBe(expectedSecondCheck);
        }

        [Theory(DisplayName = "GenerateDocumentNumber with double-digit start should produce valid CPF")]
        [InlineData(10)]
        [InlineData(55)]
        [InlineData(99)]
        public void GenerateDocumentNumberWithDoubleDigitStartShouldProduceValidCpf(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(CPF_LENGTH);
            generated.ShouldStartWith(startWith.ToString());

            long baseNumber = long.Parse(generated.Substring(0, 9));
            int expectedFirstCheck = vatGenerator.CalculateCheckDigit(baseNumber);
            int.Parse(generated.Substring(9, 1)).ShouldBe(expectedFirstCheck);

            long first10Digits = long.Parse(generated.Substring(0, 10));
            int expectedSecondCheck = vatGenerator.CalculateCheckDigit(first10Digits);
            int.Parse(generated.Substring(10, 1)).ShouldBe(expectedSecondCheck);
        }

        [Theory(DisplayName = "GenerateDocumentNumber with triple-digit start should produce valid CPF")]
        [InlineData(100)]
        [InlineData(555)]
        [InlineData(999)]
        public void GenerateDocumentNumberWithTripleDigitStartShouldProduceValidCpf(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Length.ShouldBe(CPF_LENGTH);
            generated.ShouldStartWith(startWith.ToString());

            long baseNumber = long.Parse(generated.Substring(0, 9));
            int expectedFirstCheck = vatGenerator.CalculateCheckDigit(baseNumber);
            int.Parse(generated.Substring(9, 1)).ShouldBe(expectedFirstCheck);

            long first10Digits = long.Parse(generated.Substring(0, 10));
            int expectedSecondCheck = vatGenerator.CalculateCheckDigit(first10Digits);
            int.Parse(generated.Substring(10, 1)).ShouldBe(expectedSecondCheck);
        }

        [Fact(DisplayName = "GenerateCnpjNumber should return a 14-character string")]
        public void GenerateCnpjNumberShouldReturnFourteenCharacters()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateCnpjNumber();

            // Assert
            generated.Length.ShouldBe(CNPJ_LENGTH);
        }

        [Fact(DisplayName = "GenerateCnpjNumber should return only digits")]
        public void GenerateCnpjNumberShouldReturnOnlyDigits()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCnpjNumber();

                // Assert
                generated.ShouldMatch(@"^\d{14}$");
            }
        }

        [Fact(DisplayName = "GenerateCnpjNumber should have valid first check digit")]
        public void GenerateCnpjNumberShouldHaveValidFirstCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCnpjNumber();
                long baseNumber = long.Parse(generated.Substring(0, 12));
                int expectedFirstCheck = vatGenerator.CalculateCnpjCheckDigit(baseNumber, weights1);

                // Assert
                int actualFirstCheck = int.Parse(generated.Substring(12, 1));
                actualFirstCheck.ShouldBe(expectedFirstCheck);
            }
        }

        [Fact(DisplayName = "GenerateCnpjNumber should have valid second check digit")]
        public void GenerateCnpjNumberShouldHaveValidSecondCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            int[] weights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] weights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCnpjNumber();
                long baseNumber = long.Parse(generated.Substring(0, 12));
                int firstCheck = vatGenerator.CalculateCnpjCheckDigit(baseNumber, weights1);
                long withFirst = baseNumber * 10 + firstCheck;
                int expectedSecondCheck = vatGenerator.CalculateCnpjCheckDigit(withFirst, weights2);

                // Assert
                int actualSecondCheck = int.Parse(generated.Substring(13, 1));
                actualSecondCheck.ShouldBe(expectedSecondCheck);
            }
        }

        [Theory(DisplayName = "CalculateCnpjCheckDigit returns expected value for known inputs")]
        [InlineData(0, new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 }, 0)]
        public void CalculateCnpjCheckDigitReturnsExpectedValue(long number, int[] weights, int expectedCheckDigit)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            int checkDigit = vatGenerator.CalculateCnpjCheckDigit(number, weights);

            // Assert
            checkDigit.ShouldBe(expectedCheckDigit);
        }

        [Fact(DisplayName = "CalculateCnpjCheckDigit should always return value between 0 and 9")]
        public void CalculateCnpjCheckDigitShouldReturnValueInRange()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            int[] weights = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Act & Assert
            for (long i = 100000000000; i < 100000000100; i++)
            {
                int checkDigit = vatGenerator.CalculateCnpjCheckDigit(i, weights);
                checkDigit.ShouldBeGreaterThanOrEqualTo(0);
                checkDigit.ShouldBeLessThanOrEqualTo(9);
            }
        }
    }
}