namespace DocumentNumber.Portugal.Nif.Generator
{
    using DocumentNumber.Portugal.Vat.Generator;
    using System;

#pragma warning disable S1133

    [Obsolete("This class is deprecated. Please use the VatGenerator class instead. on DocumentNumber.Portugal.Vat.Generator library")]
    public sealed class NifGenerator : INifGenerator
    {
        private readonly IVatGenerator _vatGenerator;

        public NifGenerator()
        {
            _vatGenerator = new VatGenerator();
        }

        public int CalculateCheckDigit(long uncheckedNumber)
        {
            return _vatGenerator.CalculateCheckDigit(uncheckedNumber);
        }

        public string GenerateDocumentNumber()
        {
            return _vatGenerator.GenerateDocumentNumber();
        }

        public string GenerateDocumentNumber(int startWithNumber)
        {
            return _vatGenerator.GenerateDocumentNumber(startWithNumber);
        }
    }
}