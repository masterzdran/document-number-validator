namespace DocumentNumber.Portugal.Vat.Generator
{
    using System;

    public class VatGenerator : IVatGenerator
    {
        private readonly int[] _nifGroups = { 1, 2, 3, 5, 6, 8, 45, 70, 71, 72, 77, 78, 79, 90, 98, 99 };

        public int CalculateCheckDigit(long uncheckedNumber)
        {
            long number = uncheckedNumber;
            long sum = 0;

            long remainer = 0;
            for (int factor = 2; factor < 10; factor++)
            {
                remainer = number % 10;
                sum += remainer * factor;
                number = number / 10;
            }
            var calculatedCheckDigit = sum % 11;

            var simplifiedCheckDigit = (calculatedCheckDigit == 0 || calculatedCheckDigit == 1) ? 0 : 11 - calculatedCheckDigit;
            if (simplifiedCheckDigit > 9)
            {
                simplifiedCheckDigit = simplifiedCheckDigit % 10;
            }
            return (int)simplifiedCheckDigit;
        }

        public string GenerateDocumentNumber()
        {
            Random random = new Random();
            int randomIndex = random.Next(0, _nifGroups.Length - 1);
            int startWith = _nifGroups[randomIndex];
            string generatedNif = GenerateDocumentNumber(startWith);
            return generatedNif;
        }

        public string GenerateDocumentNumber(int startWithNumber)
        {
            Random random = new Random();
            long randomNif = GenerateNifNumber(startWithNumber, random);
            long checkDigit = CalculateCheckDigit(randomNif);
            string generatedNif = $"{randomNif}{checkDigit}";
            return generatedNif;
        }

        private static long GenerateNifNumber(int expected, Random random)
        {
            int max;
            int min;
            if (expected > 10)
            {
                min = expected * 100;
                max = min + 99;
            }
            else
            {
                min = expected * 1000;
                max = min + 999;
            }

            int high = random.Next(min, max);
            int low = random.Next(1000, 9999);

            long randomNif = (high * 10000) + low;
            return randomNif;
        }
    }
}