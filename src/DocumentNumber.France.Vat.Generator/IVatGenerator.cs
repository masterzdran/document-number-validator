namespace DocumentNumber.France.Vat.Generator
{
  public interface IVatGenerator
  {
    int CalculateCheckDigit(long siren);

    string GenerateDocumentNumber();

    string GenerateDocumentNumber(int startWithNumber);
  }
}
