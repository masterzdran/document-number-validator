namespace DocumentNumber.France.Vat.Validator
{
    using DocumentNumber.France.Vat.Generator;
    using System;

    public sealed class VatValidator : IVatValidator
    {
        public bool Validate(string value)
        {
            if (value is null)
            {
                return false;
            }

            var trimmed = value.Trim();

            if (trimmed.Length != 13)
            {
                return false;
            }

            if (!trimmed.StartsWith("FR"))
            {
                return false;
            }

            string checkKeyPart = trimmed.Substring(2, 2);
            string sirenPart = trimmed.Substring(4, 9);

            if (!Int32.TryParse(checkKeyPart, out int checkKey))
            {
                return false;
            }

            if (!Int64.TryParse(sirenPart, out long siren))
            {
                return false;
            }

            IVatGenerator vatGenerator = new VatGenerator();
            int expectedCheckKey = vatGenerator.CalculateCheckDigit(siren);
            return checkKey == expectedCheckKey;
        }
    }
}
