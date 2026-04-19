namespace DocumentNumber.Brazil.Vat.Validator
{
    using DocumentNumber.Brazil.Vat.Generator;
    using System;

    public sealed class VatValidator : IVatValidator
    {
        private static readonly int[] CnpjWeights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] CnpjWeights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        public bool Validate(string value)
        {
            if (value is null)
            {
                return false;
            }

            var trimmed = value.Trim();

            if (!IsAllDigits(trimmed))
            {
                return false;
            }

            if (trimmed.Length == 11)
            {
                if (IsRepeatedDigits(trimmed))
                {
                    return false;
                }
                return ValidateCpf(trimmed);
            }

            if (trimmed.Length == 14)
            {
                if (IsRepeatedDigits(trimmed))
                {
                    return false;
                }
                return ValidateCnpj(trimmed);
            }

            return false;
        }

        private static bool ValidateCpf(string cpf)
        {
            long baseNumber = long.Parse(cpf.Substring(0, 9));

            IVatGenerator vatGenerator = new VatGenerator();

            int expectedFirstCheck = vatGenerator.CalculateCheckDigit(baseNumber);
            if (cpf[9] - '0' != expectedFirstCheck)
            {
                return false;
            }

            long numberWithFirstCheck = baseNumber * 10 + expectedFirstCheck;
            int expectedSecondCheck = vatGenerator.CalculateCheckDigit(numberWithFirstCheck);
            return cpf[10] - '0' == expectedSecondCheck;
        }

        private static bool ValidateCnpj(string cnpj)
        {
            long baseNumber = long.Parse(cnpj.Substring(0, 12));

            IVatGenerator vatGenerator = new VatGenerator();

            int expectedFirstCheck = vatGenerator.CalculateCnpjCheckDigit(baseNumber, CnpjWeights1);
            if (cnpj[12] - '0' != expectedFirstCheck)
            {
                return false;
            }

            long withFirstCheck = baseNumber * 10 + expectedFirstCheck;
            int expectedSecondCheck = vatGenerator.CalculateCnpjCheckDigit(withFirstCheck, CnpjWeights2);
            return cnpj[13] - '0' == expectedSecondCheck;
        }

        private static bool IsAllDigits(string value)
        {
            if (value.Length == 0) return false;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] < '0' || value[i] > '9')
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsRepeatedDigits(string value)
        {
            char first = value[0];
            for (int i = 1; i < value.Length; i++)
            {
                if (value[i] != first)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
