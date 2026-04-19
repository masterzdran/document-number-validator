namespace DocumentNumber.Portugal.Nif.Generator
{
    using System;

#pragma warning disable S1133

    [Obsolete("This class is deprecated. Please use the IVatGenerator class instead. on DocumentNumber.Portugal.Vat.Generator library")]
    public interface INifGenerator
    {
        int CalculateCheckDigit(long uncheckedNumber);

        string GenerateDocumentNumber();

        string GenerateDocumentNumber(int startWithNumber);
    }
}