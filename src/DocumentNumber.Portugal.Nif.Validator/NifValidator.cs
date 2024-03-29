﻿using DocumentNumber.Portugal.Nif.Generator;
using System;
using System.Collections.Generic;

namespace Portugal.Nif.Validator
{
  public sealed class NifValidator : INifValidator
  {
    ///<inheritdoc/>
    public bool Validate(string value)
    {
      if (value is null)
      {
        return false;
      }

      if (value.Trim().Length != 9)
      {
        return false;
      }

      long valueAsInt = 0;

      if (!Int64.TryParse(value, out valueAsInt))
      {
        return false;
      }

      var hasValidScopeId = HasValidScopeId(value.Trim());

      if (hasValidScopeId)
      {
        return ValidateNif(valueAsInt);
      }

      return false;
    }

    private bool ValidateNif(long value)
    {
      long checkDigit = value % 10;
      long number = (int)(value / 10);
      NifGenerator nifGenerator = new NifGenerator();
      var calculatedCheckDigit = nifGenerator.CalculateCheckDigit(number);
      return checkDigit == calculatedCheckDigit;
    }

    private bool HasValidScopeId(string nif)
    {
      var SingleCharacterScopeId = new List<string> { "1", "2", "3", "5", "6", "8" };
      var DoubleCharacterScopeId = new List<string> { "45", "70", "74", "75", "71", "72", "77", "78", "79", "90", "91", "98", "99" };
      var firstOneCharacter = nif.Substring(0, 1);
      var firstTwoCharacter = nif.Substring(0, 2);

      foreach (var c in SingleCharacterScopeId)
      {
        if (c.Equals(firstOneCharacter))
        {
          return true;
        }
      }
      foreach (var c in DoubleCharacterScopeId)
      {
        if (c.Equals(firstTwoCharacter))
        {
          return true;
        }
      }
      return false;
    }
  }
}
