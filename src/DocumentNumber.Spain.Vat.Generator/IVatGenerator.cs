namespace DocumentNumber.Spain.Vat.Generator
{
  public interface IVatGenerator
  {
    char CalculateCheckDigit(long uncheckedNumber);

    char CalculateCifControl(string cifDigits, char cifLetter);

    string GenerateDocumentNumber();

    string GenerateDocumentNumber(int startWithNumber);

    string GenerateCifNumber();
  }
}
