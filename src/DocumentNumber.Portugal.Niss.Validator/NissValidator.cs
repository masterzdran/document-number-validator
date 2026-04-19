using DocumentNumber.Portugal.Niss.Generator;
using System;

namespace DocumentNumber.Portugal.Niss.Validator
{
    public sealed class NissValidator : INissValidator
    {
        ///<inheritdoc/>
        public bool Validate(string value)
        {
            if (value is null)
            {
                return false;
            }
            if (value.Trim().Length != 11)
            {
                return false;
            }

            long valueAsInt = 0;

            if (!Int64.TryParse(value, out valueAsInt))
            {
                return false;
            }

            return ValidateNiss(valueAsInt);
        }

        private static bool ValidateNiss(long valueAsInt)
        {
            NissGenerator nissGenerator = new NissGenerator();
            int calculatedCheckDigit = nissGenerator.CalculateCheckDigit(valueAsInt / 10);
            return (valueAsInt % 10) == calculatedCheckDigit;
        }
    }
}