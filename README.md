
# ![DocumentNumber.Portugal](https://raw.githubusercontent.com/masterzdran/document-number-validator/develop/images/cards.64.png "document-number-validator") document-number-validator
A set of libraries to validate and generate document numbers for a particular document and particular country.

# Validators

## Portugal
* Portugal VAT (NIF) Number Validator
* Portugal NISS Number Validator
* Portugal Citizen Card Number Validator
* Portugal NIB Validator
* International Bank Account Number (IBAN) Validator

## Spain
* Spain VAT Number Validator (DNI, NIE, and CIF)

## France
* France VAT (TVA) Number Validator

## Brazil
* Brazil VAT Number Validator (CPF and CNPJ)

## Payment Cards
* AmericanExpress Validator
* Maestro Validator
* MaestroUK Validator
* Mastercard Validator
* VISA Validator
* VISAElectron Validator

# Generators (for testing purposes)

## Portugal
* Portugal VAT (NIF) Number Generator
* Portugal NISS Number Generator
* Portugal Citizen Card Number Generator
* Portugal BankAccountNumber Generator

## Spain
* Spain VAT Number Generator (DNI, NIE, and CIF)

## France
* France VAT (TVA) Number Generator

## Brazil
* Brazil VAT Number Generator (CPF and CNPJ)

## Payment Cards
* AmericanExpress Generator
* Maestro Generator
* MaestroUK Generator
* Mastercard Generator
* VISA Generator
* VISAElectron Generator

# Changelog
[Change log.](https://raw.githubusercontent.com/masterzdran/document-number-validator/main/CHANGELOG.md)

# Usage
Check the unit tests projects, under /tests/ for usage, 
```csharp
    IDocumentNumberValidator visaPaymentCardValidator = new VisaPaymentCardValidator();
    var result = visaPaymentCardValidator.Validate(visacard);
```
```csharp
    IDocumentNumberValidator validator = new VatValidator();
    bool validationResult = validator.Validate(nif);
```

# Contributors
* [@masterzdran](https://github.com/masterzdran)
* [@joaomatossilva](https://github.com/joaomatossilva)
   * Portugal NIB Validator
   * International Bank Account Number (IBAN) Validator


# Attribution 
Icons made by [Pixel perfect](https://icon54.com/) from [www.flaticon.com](https://www.flaticon.com/)
