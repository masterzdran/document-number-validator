using System.Text.Json.Serialization;
namespace DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig
{
  internal sealed class CountryConfig
  {
    [JsonPropertyName("code")]
    internal string Code { get; set; }
    [JsonPropertyName("length")]
    internal int Length { get; set; }
  }
}