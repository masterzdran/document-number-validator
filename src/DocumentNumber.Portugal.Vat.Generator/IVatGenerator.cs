namespace DocumentNumber.Portugal.Vat.Generator
{
  public interface IVatGenerator
  {
    int CalculateCheckDigit(long uncheckedNumber);

    string GenerateDocumentNumber();

    string GenerateDocumentNumber(int startWithNumber);
  }
}