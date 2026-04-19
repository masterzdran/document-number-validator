namespace DocumentNumber.Brazil.Vat.Generator
{
    using System;

    public class VatGenerator : IVatGenerator
    {
        private static readonly int[] CnpjWeights1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        private static readonly int[] CnpjWeights2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        public int CalculateCheckDigit(long uncheckedNumber)
        {
            long number = uncheckedNumber;
            int digitCount = CountDigits(uncheckedNumber);

            int startWeight = digitCount + 1;
            long sum = 0;

            for (int weight = 2; weight <= startWeight; weight++)
            {
                long digit = number % 10;
                sum += digit * weight;
                number = number / 10;
            }

            long remainder = sum % 11;
            int checkDigit = remainder < 2 ? 0 : (int)(11 - remainder);
            return checkDigit;
        }

        public int CalculateCnpjCheckDigit(long uncheckedNumber, int[] weights)
        {
            string digits = uncheckedNumber.ToString().PadLeft(weights.Length, '0');
            long sum = 0;

            for (int i = 0; i < weights.Length; i++)
            {
                sum += (digits[i] - '0') * weights[i];
            }

            long remainder = sum % 11;
            return remainder < 2 ? 0 : (int)(11 - remainder);
        }

        public string GenerateDocumentNumber()
        {
            Random random = new Random();
            int startWith = random.Next(1, 9);
            return GenerateDocumentNumber(startWith);
        }

        public string GenerateDocumentNumber(int startWithNumber)
        {
            Random random = new Random();
            long baseNumber = GenerateBaseNumber(startWithNumber, random);

            int firstCheckDigit = CalculateCheckDigit(baseNumber);
            long numberWithFirstCheck = baseNumber * 10 + firstCheckDigit;

            int secondCheckDigit = CalculateCheckDigit(numberWithFirstCheck);
            string cpf = $"{baseNumber:D9}{firstCheckDigit}{secondCheckDigit}";
            return cpf;
        }

        public string GenerateCnpjNumber()
        {
            Random random = new Random();
            long baseNumber = GenerateCnpjBaseNumber(random);

            int firstCheck = CalculateCnpjCheckDigit(baseNumber, CnpjWeights1);
            long withFirstCheck = baseNumber * 10 + firstCheck;

            int secondCheck = CalculateCnpjCheckDigit(withFirstCheck, CnpjWeights2);
            return $"{baseNumber:D12}{firstCheck}{secondCheck}";
        }

        private static long GenerateCnpjBaseNumber(Random random)
        {
            // CNPJ base: 8 digits (company) + 4 digits (branch, typically 0001 for main)
            long company = random.Next(10000000, 99999999);
            int branch = random.Next(1, 9999);
            return company * 10000L + branch;
        }

        private static long GenerateBaseNumber(int startWith, Random random)
        {
            int remaining;
            if (startWith >= 100)
            {
                remaining = random.Next(100000, 999999);
                return startWith * 1000000L + remaining;
            }
            else if (startWith >= 10)
            {
                remaining = random.Next(1000000, 9999999);
                return startWith * 10000000L + remaining;
            }
            else
            {
                remaining = random.Next(10000000, 99999999);
                return startWith * 100000000L + remaining;
            }
        }

        private static int CountDigits(long number)
        {
            if (number == 0) return 1;
            int count = 0;
            while (number > 0)
            {
                count++;
                number = number / 10;
            }
            return count;
        }
    }
}
