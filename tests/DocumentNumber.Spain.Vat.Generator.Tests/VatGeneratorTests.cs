namespace DocumentNumber.Spain.Vat.Generator.Tests
{
    using DocumentNumber.Spain.Vat.Generator;
    using Shouldly;
    using Xunit;

    public class VatGeneratorTests
    {
        private const string CheckDigitLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

        [Theory(DisplayName = "CalculateCheckDigit returns expected letter for known DNI numbers")]
        [InlineData(12345678, 'Z')]
        [InlineData(0, 'T')]
        [InlineData(23, 'T')]
        [InlineData(1, 'R')]
        [InlineData(22, 'E')]
        public void CalculateCheckDigitReturnsExpectedLetter(long number, char expectedLetter)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            char checkDigit = vatGenerator.CalculateCheckDigit(number);

            // Assert
            checkDigit.ShouldBe(expectedLetter);
        }

        [Fact(DisplayName = "CalculateCheckDigit letter is always within valid check digit letters")]
        public void CalculateCheckDigitLetterIsAlwaysValid()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act & Assert
            for (int i = 0; i < 23; i++)
            {
                char checkDigit = vatGenerator.CalculateCheckDigit(i);
                CheckDigitLetters.ShouldContain(checkDigit);
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

        [Fact(DisplayName = "GenerateDocumentNumber should return a 9-character string")]
        public void GenerateDocumentNumberShouldReturnNineCharacters()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber();

            // Assert
            generated.Length.ShouldBe(9);
        }

        [Fact(DisplayName = "GenerateDocumentNumber should produce a valid DNI or NIE format")]
        public void GenerateDocumentNumberShouldProduceValidFormat()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();

                // Assert
                generated.Length.ShouldBe(9);

                char firstChar = generated[0];
                string cifLetters = "ABCDEFGHJNPQRSUVW";
                if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
                {
                    // NIE format: letter + 7 digits + check letter
                    generated.Substring(1, 7).ShouldMatch(@"^\d{7}$");
                    CheckDigitLetters.ShouldContain(generated[8]);
                }
                else if (cifLetters.IndexOf(firstChar) >= 0)
                {
                    // CIF format: letter + 7 digits + control
                    generated.Substring(1, 7).ShouldMatch(@"^\d{7}$");
                }
                else
                {
                    // DNI format: 8 digits + check letter
                    generated.Substring(0, 8).ShouldMatch(@"^\d{8}$");
                    CheckDigitLetters.ShouldContain(generated[8]);
                }
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber produces DNI with correct check digit")]
        public void GenerateDocumentNumberDniHasCorrectCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();

                // Assert
                char firstChar = generated[0];
                if (firstChar >= '0' && firstChar <= '9')
                {
                    long number = long.Parse(generated.Substring(0, 8));
                    char expectedCheckDigit = vatGenerator.CalculateCheckDigit(number);
                    generated[8].ShouldBe(expectedCheckDigit);
                }
            }
        }

        [Fact(DisplayName = "GenerateDocumentNumber produces NIE with correct check digit")]
        public void GenerateDocumentNumberNieHasCorrectCheckDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 100; i++)
            {
                // Act
                string generated = vatGenerator.GenerateDocumentNumber();

                // Assert
                char firstChar = generated[0];
                if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
                {
                    int prefixValue = firstChar == 'X' ? 0 : firstChar == 'Y' ? 1 : 2;
                    long digits = long.Parse(generated.Substring(1, 7));
                    long fullNumber = prefixValue * 10000000L + digits;
                    char expectedCheckDigit = vatGenerator.CalculateCheckDigit(fullNumber);
                    generated[8].ShouldBe(expectedCheckDigit);
                }
            }
        }

        [Theory(DisplayName = "GenerateDocumentNumber with startWithNumber should start with that number")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(12)]
        [InlineData(45)]
        [InlineData(99)]
        public void GenerateDocumentNumberWithStartShouldStartWithNumber(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.ShouldNotBeNullOrEmpty();
            generated.Length.ShouldBe(9);
            generated.ShouldStartWith(startWith.ToString());
        }

        [Theory(DisplayName = "GenerateDocumentNumber with startWithNumber should have valid check digit")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        [InlineData(12)]
        [InlineData(45)]
        [InlineData(99)]
        public void GenerateDocumentNumberWithStartShouldHaveValidCheckDigit(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            long number = long.Parse(generated.Substring(0, 8));
            char expectedCheckDigit = vatGenerator.CalculateCheckDigit(number);
            generated[8].ShouldBe(expectedCheckDigit);
        }

        [Theory(DisplayName = "GenerateDocumentNumber with single-digit start produces 8-digit number portion")]
        [InlineData(1)]
        [InlineData(9)]
        public void GenerateDocumentNumberSingleDigitStartProducesEightDigitNumber(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Substring(0, 8).ShouldMatch(@"^\d{8}$");
        }

        [Theory(DisplayName = "GenerateDocumentNumber with double-digit start produces 8-digit number portion")]
        [InlineData(10)]
        [InlineData(45)]
        [InlineData(99)]
        public void GenerateDocumentNumberDoubleDigitStartProducesEightDigitNumber(int startWith)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateDocumentNumber(startWith);

            // Assert
            generated.Substring(0, 8).ShouldMatch(@"^\d{8}$");
        }

        [Fact(DisplayName = "GenerateCifNumber should return a 9-character string")]
        public void GenerateCifNumberShouldReturnNineCharacters()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            string generated = vatGenerator.GenerateCifNumber();

            // Assert
            generated.Length.ShouldBe(9);
        }

        [Fact(DisplayName = "GenerateCifNumber should start with a valid CIF letter")]
        public void GenerateCifNumberShouldStartWithValidCifLetter()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            string validLetters = "ABCDEFGHJNPQRSUVW";

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCifNumber();

                // Assert
                validLetters.ShouldContain(generated[0]);
            }
        }

        [Fact(DisplayName = "GenerateCifNumber should have 7 digits in the middle")]
        public void GenerateCifNumberShouldHaveSevenDigitsInMiddle()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCifNumber();

                // Assert
                generated.Substring(1, 7).ShouldMatch(@"^\d{7}$");
            }
        }

        [Fact(DisplayName = "GenerateCifNumber with letter-control type should end with a letter")]
        public void GenerateCifNumberLetterControlTypeShouldEndWithLetter()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            string letterControlTypes = "NPQRSW";
            string controlLetters = "JABCDEFGHI";

            for (int i = 0; i < 200; i++)
            {
                string generated = vatGenerator.GenerateCifNumber();
                char cifLetter = generated[0];

                if (letterControlTypes.IndexOf(cifLetter) >= 0)
                {
                    // Assert: last char should be a control letter
                    controlLetters.ShouldContain(generated[8]);
                }
            }
        }

        [Fact(DisplayName = "GenerateCifNumber with digit-control type should end with a digit")]
        public void GenerateCifNumberDigitControlTypeShouldEndWithDigit()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            string letterControlTypes = "NPQRSW";

            for (int i = 0; i < 200; i++)
            {
                string generated = vatGenerator.GenerateCifNumber();
                char cifLetter = generated[0];

                if (letterControlTypes.IndexOf(cifLetter) < 0)
                {
                    // Assert: last char should be a digit
                    char lastChar = generated[8];
                    (lastChar >= '0' && lastChar <= '9').ShouldBeTrue();
                }
            }
        }

        [Fact(DisplayName = "GenerateCifNumber control character should match CalculateCifControl")]
        public void GenerateCifNumberControlShouldMatchCalculation()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            for (int i = 0; i < 100; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCifNumber();
                char cifLetter = generated[0];
                string digitsPart = generated.Substring(1, 7);
                char expectedControl = vatGenerator.CalculateCifControl(digitsPart, cifLetter);

                // Assert
                generated[8].ShouldBe(expectedControl);
            }
        }

        [Theory(DisplayName = "CalculateCifControl returns expected control for known CIF values")]
        [InlineData("1234567", 'A', '4')]
        [InlineData("0000000", 'A', '0')]
        [InlineData("0000000", 'N', 'J')]
        public void CalculateCifControlReturnsExpectedValue(string digits, char cifLetter, char expectedControl)
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();

            // Act
            char control = vatGenerator.CalculateCifControl(digits, cifLetter);

            // Assert
            control.ShouldBe(expectedControl);
        }

        [Fact(DisplayName = "GenerateCifNumber produces valid CIF that passes validation")]
        public void GenerateCifNumberProducesValidCif()
        {
            // Arrange
            IVatGenerator vatGenerator = new VatGenerator();
            var validator = new Spain.Vat.Validator.VatValidator();

            for (int i = 0; i < 50; i++)
            {
                // Act
                string generated = vatGenerator.GenerateCifNumber();

                // Assert
                validator.Validate(generated).ShouldBeTrue();
            }
        }
    }
}
