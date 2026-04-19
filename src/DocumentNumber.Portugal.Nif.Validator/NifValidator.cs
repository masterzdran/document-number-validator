using DocumentNumber.Portugal.Nif.Generator;
using DocumentNumber.Portugal.Vat.Validator;
using System;
using System.Collections.Generic;

namespace Portugal.Nif.Validator
{
#pragma warning disable S1133

    [Obsolete("NifValidator is obsolete. Use VatValidator instead, from DocumentNumber.Portugal.Vat.Validator library.")]
    public sealed class NifValidator : INifValidator
    {
        private readonly IVatValidator _vatValidator;

        public NifValidator()
        {
            _vatValidator = new VatValidator();
        }

        ///<inheritdoc/>
        public bool Validate(string value)
        {
            return _vatValidator.Validate(value);
        }
    }
}