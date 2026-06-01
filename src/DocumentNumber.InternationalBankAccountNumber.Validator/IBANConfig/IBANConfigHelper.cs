using System;
using System.IO;
using System.Text;
namespace DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig
{
  internal static class IBANConfigHelper
  {
    private static readonly IBANConfig _config;

    static IBANConfigHelper()
    {
        string configPath=Path.Combine(AppContext.BaseDirectory, "iban-lengths.json");
        if(!File.Exists(configPath))
          throw new FileNotFoundException($"IBAN configuration file not found at path: {configPath}");
        string json = File.ReadAllText(path: configPath, encoding: Encoding.UTF8);
        if(string.IsNullOrWhiteSpace(json))
          throw new InvalidDataException($"IBAN configuration file is empty at path: {configPath}");
        _config = System.Text.Json.JsonSerializer.Deserialize<IBANConfig>(json);
        if(_config == null) 
          throw new InvalidDataException("Failed to deserialize IBAN configuration. The resulting object is null.");
    }
    internal static IBANConfig IBANConfig => _config;
  }
}