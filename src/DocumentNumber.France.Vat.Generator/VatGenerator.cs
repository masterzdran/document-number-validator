namespace DocumentNumber.France.Vat.Generator
{
    using System;

    public class VatGenerator : IVatGenerator
    {
        public int CalculateCheckDigit(long siren)
        {
            return (int)((12 + 3 * (siren % 97)) % 97);
        }

        public string GenerateDocumentNumber()
        {
            Random random = new Random();
            long siren = GenerateSirenNumber(random);
            int checkKey = CalculateCheckDigit(siren);
            return $"FR{checkKey:D2}{siren:D9}";
        }

        public string GenerateDocumentNumber(int startWithNumber)
        {
            Random random = new Random();
            long siren = GenerateSirenNumber(startWithNumber, random);
            int checkKey = CalculateCheckDigit(siren);
            return $"FR{checkKey:D2}{siren:D9}";
        }

        private static long GenerateSirenNumber(Random random)
        {
            int high = random.Next(100, 999);
            int mid = random.Next(100, 999);
            int low = random.Next(100, 999);
            return high * 1000000L + mid * 1000L + low;
        }

        private static long GenerateSirenNumber(int startWith, Random random)
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
    }
}
