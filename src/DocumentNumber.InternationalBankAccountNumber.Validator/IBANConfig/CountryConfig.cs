using System.Text.Json.Serialization;
namespace DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig
{
  internal sealed class CountryConfig
  {
    [JsonPropertyName("code")]
    public string Code { get; set; }
    [JsonPropertyName("length")]
    public int Length { get; set; }
  }
}