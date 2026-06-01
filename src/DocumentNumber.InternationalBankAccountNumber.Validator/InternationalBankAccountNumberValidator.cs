using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig;

namespace DocumentNumber.InternationalBankAccountNumber.Validator
{
  public class InternationalBankAccountNumberValidator : IInternationalBankAccountNumberValidator
  {
    protected static readonly Regex SanitizeRegEx = new Regex("[^A-Z0-9]+");
    protected static readonly Regex IBANStructure = new Regex("^([A-Z]{2}[0-9]{2})([A-Z0-9]+)");
    protected static readonly Regex AlphaCharacters = new Regex("([A-Z])");
   

    public bool Validate(string value)
    {
      if(String.IsNullOrEmpty(value))
      {
        return false;
      }

      var sanitizedValue = Sanitize(value);
      if(!IsValid(sanitizedValue))
      {
        return ValidateNonIBAN(sanitizedValue);
      }

      var ibanMatch = IBANStructure.Match(sanitizedValue);
      if(!ibanMatch.Success)
      {
        return ValidateNonIBAN(sanitizedValue);
      }

      string countryPart = ibanMatch.Groups[1].Value;
      if(!ValidateIBANLength(countryPart, sanitizedValue.Length))
        return false;
      if (!ValidateCountry(countryPart))
      {
        return false;
      }

      var domesticAccountNumber = ibanMatch.Groups[2].Value;
      var replacedDomestic = AlphaCharacters.Replace(domesticAccountNumber, EvaluateAlphaReplace);
      if (!ValidateDomesticAccountNumber(replacedDomestic))
      {
        return false;
      }

      var replacedCountry = AlphaCharacters.Replace(countryPart, EvaluateAlphaReplace);
      var fullCheckString = replacedDomestic + replacedCountry;

      return ValidateIntegrity(fullCheckString);
    }
    /// <summary>
    /// Validates the length of the provided IBAN against the expected length for the country code as defined in the configuration file.
    /// </summary>
    /// <param name="countryPart">The country code part of the IBAN.</param>
    /// <param name="ibanLength">The length of the full IBAN to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws an exception if the IBAN length is invalid for the country code.</exception>
    /// <exception cref="NullReferenceException">Throws an exception if the country code is not supported or does not exist in the configuration.</exception>
    private bool ValidateIBANLength(string countryPart,int ibanLength)
    {
        IBANConfigHelper.LoadConfig();
        int? expectedLength = IBANConfigHelper.IBANConfig.Countries.FirstOrDefault(country=>country.Code == countryPart).Length;
        if (expectedLength == null || ibanLength != expectedLength)
            return false;
        else
            return true;
    }
    /// <summary>
    /// Allows Subclasses to perform validation for Non IBAN, full domestic BBAN
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual bool ValidateNonIBAN(string value) => false;

    /// <summary>
    /// Allows Subclasses to Perform validation on Domestic part of IBAN.
    /// Value has already the alpha characters converted to numeric
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual bool ValidateDomesticAccountNumber(string value) => true;

    /// <summary>
    /// Allows Subclasses to perform validation on the country part of IBAN
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected virtual bool ValidateCountry(string value) => true;

    /// <summary>
    /// Apply MOD 97-10 algorithm from ISO/IEC7064:2003
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected bool ValidateIntegrity(string value)
    {
      var carryDigits = 0;
      var lengthCount = 0;
      uint mod = 0;
      do
      {
        var take = Math.Min(9 - carryDigits, value.Length - lengthCount);
        var subset = value.Substring(lengthCount, take);
        var parsed = (uint)(mod * Math.Pow(10, take)) + uint.Parse(subset);
        mod = parsed % 97;
        carryDigits = 2;
        lengthCount += take;
      } while (lengthCount < value.Length);

      return mod == 1;
    }

    /// <summary>
    /// Delegate to be used for Replacing Alpha charaters into numerical values.
    /// A - 10
    /// B - 11
    /// C - 12
    /// D - 14
    /// ....
    /// Z - 35
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    protected static string EvaluateAlphaReplace(Match match)
    {
      if(!match.Success)
      {
        return match.Value;
      }

      var value = match.Value[0] - 'A' + 10;
      return value.ToString();
    }

    /// <summary>
    /// At most an IBAN can't have more than 34 characters
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private bool IsValid(string value)
    {
      if(string.IsNullOrWhiteSpace(value))
      {
        return false;
      }

      if(value.Length > 34)
      {
        return false;
      }

      return true;
    }

    /// <summary>
    /// Remove all non alpha-numeric values from the string
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private string Sanitize(string value) => SanitizeRegEx.Replace(value, String.Empty);
  }
}
