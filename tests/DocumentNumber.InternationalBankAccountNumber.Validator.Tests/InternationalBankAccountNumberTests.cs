using Xunit;
using Shouldly;

namespace DocumentNumber.InternationalBankAccountNumber.Validator
{
  public class InternationalBankAccountNumberTests
  {
    [Theory(DisplayName = "IBAN should be Invalid for Null or empty input.")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void IBAN_Should_be_Invalid_For_Null_Or_Empty_Input(string input)
    {
      // Arrange
      var subject = new InternationalBankAccountNumberValidator();

      // Act
      var result = subject.Validate(input);

      // Assert
      result.ShouldBeFalse();
    }

    [Theory(DisplayName = "IBAN Should be Invalid For Value with more than 34 characters.")]
    [InlineData("PT505678901234567890123456789012345")]
    [InlineData("PT50567 8901 2345 67890 1234 567890-12345")]
    public void IBAN_Should_be_Invalid_For_Value_with_more_than_34_characters(string value)
    {
      // Arrange
      var validator = new InternationalBankAccountNumberValidator();

      // Act
      var result = validator.Validate(value);

      // Assert
      result.ShouldBeFalse();
    }

    [Theory(DisplayName = "IBAN Should be Invalid For Value With no Country code.")]
    [InlineData("1250123443211234567890172")]
    [InlineData("1250 1234 4321-12345678901_72")]
    public void IBAN_Should_be_Invalid_For_Value_with_no_Country_Code(string value)
    {
      // Arrange
      var validator = new InternationalBankAccountNumberValidator();

      // Act
      var result = validator.Validate(value);

      // Assert
      result.ShouldBeFalse();
    }

    [Theory(DisplayName = "IBAN Should be Invalid For unsupported country code.")]
    [InlineData("ZZ00 1234 5678 9012 3456 7890 12")]
    public void IBAN_Should_be_Invalid_For_Unsupported_Country_Code(string value)
    {
      // Arrange
      var validator = new InternationalBankAccountNumberValidator();

      // Act
      var result = validator.Validate(value);

      // Assert
      result.ShouldBeFalse();
    }

    [Theory(DisplayName = "IBAN Should be Invalid For wrong country-specific length.")]
    [InlineData("PT50 1234 4321 1234 5678 9017")]
    public void IBAN_Should_be_Invalid_For_Wrong_Country_Specific_Length(string value)
    {
      // Arrange
      var validator = new InternationalBankAccountNumberValidator();

      // Act
      var result = validator.Validate(value);

      // Assert
      result.ShouldBeFalse();
    }

    [Theory(DisplayName = "IBAN Should be Valid For Valid values.")]
    [InlineData("PT50123443211234567890172")]
    [InlineData("PT50000100001234567890194")]
    [InlineData("PT50 1234 4321-1234 5678 9017_2")]
    [InlineData("GB29 NWBK 6016 1331 9268 19")]
    [InlineData("ES91 2100 0418 4502 0005 1332")]
    [InlineData("FR14 2004 1010 0505 0001 3M02 606")]
    public void IBAN_Should_be_Valid_For_valid_Values(string value)
    {
      // Arrange
      var validator = new InternationalBankAccountNumberValidator();

      // Act
      var result = validator.Validate(value);

      // Assert
      result.ShouldBeTrue();
    }
  }
}
