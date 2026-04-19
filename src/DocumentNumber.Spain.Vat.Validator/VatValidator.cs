namespace DocumentNumber.Spain.Vat.Validator
{
    using DocumentNumber.Spain.Vat.Generator;
    using System;
    using System.Collections.Generic;

    public sealed class VatValidator : IVatValidator
    {
        private static readonly HashSet<char> CifLetters = new HashSet<char> { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'N', 'P', 'Q', 'R', 'S', 'U', 'V', 'W' };

        public bool Validate(string value)
        {
            if (value is null)
            {
                return false;
            }

            var trimmed = value.Trim();

            if (trimmed.Length != 9)
            {
                return false;
            }

            char firstChar = trimmed[0];

            if (firstChar >= '0' && firstChar <= '9')
            {
                return ValidateDni(trimmed);
            }

            if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
            {
                return ValidateNie(trimmed);
            }

            if (CifLetters.Contains(firstChar))
            {
                return ValidateCif(trimmed);
            }

            return false;
        }

        private static bool ValidateDni(string value)
        {
            string digits = value.Substring(0, 8);

            if (!Int64.TryParse(digits, out long number))
            {
                return false;
            }

            IVatGenerator vatGenerator = new VatGenerator();
            char expectedCheckDigit = vatGenerator.CalculateCheckDigit(number);
            return value[8] == expectedCheckDigit;
        }

        private static bool ValidateNie(string value)
        {
            char prefix = value[0];
            int prefixValue = prefix == 'X' ? 0 : prefix == 'Y' ? 1 : 2;

            string digits = value.Substring(1, 7);

            if (!Int64.TryParse(digits, out long number))
            {
                return false;
            }

            long fullNumber = prefixValue * 10000000L + number;

            IVatGenerator vatGenerator = new VatGenerator();
            char expectedCheckDigit = vatGenerator.CalculateCheckDigit(fullNumber);
            return value[8] == expectedCheckDigit;
        }

        private static bool ValidateCif(string value)
        {
            char cifLetter = value[0];
            string digitsPart = value.Substring(1, 7);

            for (int i = 0; i < digitsPart.Length; i++)
            {
                if (digitsPart[i] < '0' || digitsPart[i] > '9')
                {
                    return false;
                }
            }

            IVatGenerator vatGenerator = new VatGenerator();
            char expectedControl = vatGenerator.CalculateCifControl(digitsPart, cifLetter);
            return value[8] == expectedControl;
        }
    }
}