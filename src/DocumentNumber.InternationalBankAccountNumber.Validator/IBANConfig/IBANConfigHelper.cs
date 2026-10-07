using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
namespace DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig
{
    /// <summary>
    /// WARNING:
    /// This configuration is loaded from an embedded resource inside the library assembly.
    /// It is not an application config file and must not be read from AppContext.BaseDirectory.
    /// Libraries must not depend on the consuming application's output folder.
    /// If runtime mutation is required, the consuming app should provide the configuration explicitly.
    /// </summary>
    internal static class IBANConfigHelper
    {
        private const string ResourceName = "DocumentNumber.InternationalBankAccountNumber.Validator.IBANConfig.IBANCountryConfiguration.json";
        private static readonly Lazy<IBANConfig> Config = new Lazy<IBANConfig>(() => LoadConfigFromResource()); //Lazy initialization of the IBAN resource, this is passed as a delegate to the Lazy constructor,
                                                                                                                //which will only be executed once when the Value property is accessed for the first time.
        internal static IBANConfig IBANConfig => Config.Value;
        /// <summary>
        /// This method loads the IBAN configuration from an embedded JSON resource file. 
        /// It reads the resource stream, deserializes the JSON content into an IBANConfig object, and returns it. 
        /// If the resource cannot be found, is empty, or fails to deserialize, appropriate exceptions are thrown.
        /// </summary>
        /// <returns>Returns the IBAN configuration.</returns>
        /// <exception cref="FileNotFoundException">Throws if the embedded resource cannot be found.</exception>
        /// <exception cref="InvalidDataException">Throws if the embedded resource is empty, whitespace, or fails to deserialize.</exception>
        private static IBANConfig LoadConfigFromResource()
        {
            Assembly assembly = typeof(IBANConfigHelper).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(ResourceName))
            {
                if (stream == null)
                    throw new FileNotFoundException($"Could not find embedded resource '{ResourceName}' in assembly.");
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    string json = reader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(json))
                        throw new InvalidDataException($"The embedded resource '{ResourceName}' is empty or whitespace.");
                    IBANConfig config = JsonSerializer.Deserialize<IBANConfig>(json);
                    if (config == null)
                        throw new InvalidDataException("Failed to deserialize IBAN configuration.");
                    return config;
                }
            }
        }
    }
}