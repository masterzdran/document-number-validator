namespace DocumentNumber.Brazil.Vat.Generator
{
  public interface IVatGenerator
  {
    int CalculateCheckDigit(long uncheckedNumber);

    int CalculateCnpjCheckDigit(long uncheckedNumber, int[] weights);

    string GenerateDocumentNumber();

    string GenerateDocumentNumber(int startWithNumber);

    string GenerateCnpjNumber();
  }
}
