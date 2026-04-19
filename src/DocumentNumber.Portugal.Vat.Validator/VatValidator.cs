namespace DocumentNumber.Portugal.Vat.Validator
{
    using DocumentNumber.Portugal.Vat.Generator;
    using System;
    using System.Collections.Generic;

    public sealed class VatValidator : IVatValidator
    {
        private static readonly HashSet<char> SingleCharScopeIds = new HashSet<char> { '1', '2', '3', '5', '6', '8' };
        private static readonly HashSet<string> DoubleCharScopeIds = new HashSet<string> { "45", "70", "71", "72", "74", "75", "77", "78", "79", "90", "91", "98", "99" };

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

            if (!Int64.TryParse(trimmed, out long valueAsInt))
            {
                return false;
            }

            return HasValidScopeId(trimmed) && ValidateNif(valueAsInt);
        }

        private static bool ValidateNif(long value)
        {
            long checkDigit = value % 10;
            long number = value / 10;
            IVatGenerator vatGenerator = new VatGenerator();
            var calculatedCheckDigit = vatGenerator.CalculateCheckDigit(number);
            return checkDigit == calculatedCheckDigit;
        }

        private static bool HasValidScopeId(string nif)
        {
            return SingleCharScopeIds.Contains(nif[0])
                || DoubleCharScopeIds.Contains(nif.Substring(0, 2));
        }
    }
}