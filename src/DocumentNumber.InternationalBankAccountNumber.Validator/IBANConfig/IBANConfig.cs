using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig
{
  internal sealed class IBANConfig
  {
    [JsonPropertyName("countries")]
    internal List<CountryConfig> Countries {  get; set; }
  }
}