using DocumentNumber.ValidatorAbstractions;
using System;

namespace Portugal.Nif.Validator
{
#pragma warning disable S1133

    [Obsolete("INifValidator is obsolete. Use IVatValidator instead, from DocumentNumber.Portugal.Vat.Validator library.")]
    public interface INifValidator : IDocumentNumberValidator
    {
    }
}