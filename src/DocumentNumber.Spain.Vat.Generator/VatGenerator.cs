namespace DocumentNumber.Spain.Vat.Generator
{
    using System;

    public class VatGenerator : IVatGenerator
    {
        private const string CheckDigitLetters = "TRWAGMYFPDXBNJZSQVHLCKE";
        private static readonly string[] _nieLetters = { "X", "Y", "Z" };
        private static readonly char[] _cifLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'N', 'P', 'Q', 'R', 'S', 'U', 'V', 'W' };
        private const string CifControlLetters = "JABCDEFGHI";
        // CIF types that require a letter as control character
        private static readonly string _cifLetterControl = "NPQRSW";

        public char CalculateCheckDigit(long uncheckedNumber)
        {
            int remainder = (int)(uncheckedNumber % 23);
            return CheckDigitLetters[remainder];
        }

        public char CalculateCifControl(string cifDigits, char cifLetter)
        {
            int sumEven = 0;
            int sumOdd = 0;

            for (int i = 0; i < 7; i++)
            {
                int digit = cifDigits[i] - '0';
                if ((i + 1) % 2 == 0)
                {
                    sumEven += digit;
                }
                else
                {
                    int doubled = digit * 2;
                    sumOdd += (doubled / 10) + (doubled % 10);
                }
            }

            int total = sumEven + sumOdd;
            int control = (10 - (total % 10)) % 10;

            if (_cifLetterControl.IndexOf(cifLetter) >= 0)
            {
                return CifControlLetters[control];
            }
            return (char)('0' + control);
        }

        public string GenerateDocumentNumber()
        {
            Random random = new Random();
            int type = random.Next(0, 3);

            if (type == 0)
            {
                return GenerateDni(random);
            }
            else if (type == 1)
            {
                return GenerateNie(random);
            }
            else
            {
                return GenerateCif(random);
            }
        }

        public string GenerateDocumentNumber(int startWithNumber)
        {
            Random random = new Random();
            long number = GenerateDniNumber(startWithNumber, random);
            char checkDigit = CalculateCheckDigit(number);
            return $"{number:D8}{checkDigit}";
        }

        public string GenerateCifNumber()
        {
            Random random = new Random();
            return GenerateCif(random);
        }

        private string GenerateDni(Random random)
        {
            long number = random.Next(10000000, 99999999);
            char checkDigit = CalculateCheckDigit(number);
            return $"{number:D8}{checkDigit}";
        }

        private string GenerateNie(Random random)
        {
            int prefixIndex = random.Next(0, _nieLetters.Length);
            string prefix = _nieLetters[prefixIndex];
            int digits = random.Next(1000000, 9999999);
            long fullNumber = prefixIndex * 10000000L + digits;
            char checkDigit = CalculateCheckDigit(fullNumber);
            return $"{prefix}{digits:D7}{checkDigit}";
        }

        private string GenerateCif(Random random)
        {
            int letterIndex = random.Next(0, _cifLetters.Length);
            char cifLetter = _cifLetters[letterIndex];
            int digits = random.Next(1000000, 9999999);
            string digitsPart = digits.ToString("D7");
            char control = CalculateCifControl(digitsPart, cifLetter);
            return $"{cifLetter}{digitsPart}{control}";
        }

        private static long GenerateDniNumber(int startWith, Random random)
        {
            int remaining;
            if (startWith >= 10)
            {
                remaining = random.Next(100000, 999999);
                return startWith * 1000000L + remaining;
            }
            else
            {
                remaining = random.Next(1000000, 9999999);
                return startWith * 10000000L + remaining;
            }
        }
    }
}
