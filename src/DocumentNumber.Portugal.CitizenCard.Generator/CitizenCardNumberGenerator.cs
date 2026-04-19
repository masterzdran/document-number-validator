namespace DocumentNumber.Portugal.CitizenCard.Generator
{
    using System;

    public class CitizenCardNumberGenerator : ICitizenCardNumberGenerator
    {
        public int CalculateCheckDigit(string uncheckedNumber)
        {
            string value = uncheckedNumber.Trim();

            int sum = 0;
            for (int index = 0; index < value.Length; index++)
            {
                int translatedValue = TranslatedValue(value[index]);
                if ((index % 2) == 0)
                {
                    translatedValue *= 2;
                    if (translatedValue > 9)
                    {
                        translatedValue -= 9;
                    }
                }
                sum += translatedValue;
            }
            int checkdigit = (10 - (sum % 10)) % 10;

            return checkdigit;
        }

        private static int TranslatedValue(char c)
        {
            const int CharTranslationValue = 55;
            const int CharTranslationNumberValue = 48;
            if (c >= '0' && c <= '9')
            {
                return c - CharTranslationNumberValue;
            }
            return c - CharTranslationValue;
        }

        public string GenerateDocumentNumber()
        {
            string documentId = string.Empty;

            string generatedId = GenerateDocumentNumberId();
            string generatedIdCheckDigit = CalculateDocumentNumberIdCheckDigit(generatedId);
            string generateVersionId = GenerateDocumentVersionId();
            string uncheckedId = $"{generatedId}{generatedIdCheckDigit}{generateVersionId}";

            int documentCheckDigit = CalculateCheckDigit(uncheckedId);
            documentId = $"{uncheckedId}{documentCheckDigit}";
            return documentId;
        }

        private string GenerateDocumentVersionId()
        {
            Random random = new Random();
            char randomChar1 = (char)random.Next('A', 'Z');
            char randomChar2 = (char)random.Next('A', 'Z');
            string version = $"{randomChar1}{randomChar2}";
            return version;
        }

        private string CalculateDocumentNumberIdCheckDigit(string generatedId)
        {
            long checkdigit = 0;
            long prod = 2;

            var valueReduced = Int64.Parse(generatedId);
            long total = 0;
            while (valueReduced != 0)
            {
                long unit = valueReduced % 10;
                valueReduced = valueReduced / 10;
                total += unit * prod++;
            }
            long module = total % 11;
            checkdigit = 11 - module;
            checkdigit = checkdigit % 10;
            return $"{checkdigit}";
        }

        private string GenerateDocumentNumberId()
        {
            Random random = new Random();
            int high = random.Next(1000, 9999);
            int low = random.Next(1000, 9999);

            return $"{high}{low}";
        }
    }
}