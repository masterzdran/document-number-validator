# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.7.1] - 2026-06-02
### Fixed
- DocumentNumber.InternationalBankAccountNumber - Fixed incorrect check digit calculation for certain IBANs, which could lead to false negatives in validation.

## [1.7.0] - 2026-04-19
### Added
- DocumentNumber.Portugal.Vat.Generator - Portugal VAT (NIF) number generator.
- DocumentNumber.Portugal.Vat.Validator - Portugal VAT (NIF) number validator.
- DocumentNumber.Spain.Vat.Generator - Spain VAT number generator (DNI, NIE, and CIF).
- DocumentNumber.Spain.Vat.Validator - Spain VAT number validator (DNI, NIE, and CIF).
- DocumentNumber.France.Vat.Generator - France VAT (TVA) number generator.
- DocumentNumber.France.Vat.Validator - France VAT (TVA) number validator.
- DocumentNumber.Brazil.Vat.Generator - Brazil VAT number generator (CPF and CNPJ).
- DocumentNumber.Brazil.Vat.Validator - Brazil VAT number validator (CPF and CNPJ).

### Changed
- Portugal NIF Generator and Validator renamed to VAT Generator and Validator.

### Deprecated
- DocumentNumber.Portugal.Nif.Generator - Use DocumentNumber.Portugal.Vat.Generator instead.
- DocumentNumber.Portugal.Nif.Validator - Use DocumentNumber.Portugal.Vat.Validator instead.

### Fixed
- Portugal CitizenCardNumberGenerator.CalculateCheckDigit - Fixed trim/index mismatch and check-digit-of-10 bug.
- Portugal VatValidator.Validate - Replaced per-call List allocations with static readonly HashSet, eliminated duplicate Trim calls, and simplified control flow.



## [1.6.0] - 2023-07-18
### Changed
- Project are references instead of nuget references.
- Update libraries to the latest version.
- Target Framework to netstandard 2.0.
- Test targeting net6.0, net7.0, net462, net472, net48, net481

## [1.5.0] - 2022-07-08
### Added
- DocumentNumber.PaymentCardNumber.AmericanExpress.Generator (for testing purposes)
- DocumentNumber.PaymentCardNumber.Maestro.Generator (for testing purposes)
- DocumentNumber.PaymentCardNumber.MaestroUK.Generator (for testing purposes)
- DocumentNumber.PaymentCardNumber.Mastercard.Generator (for testing purposes)
- DocumentNumber.PaymentCardNumber.VISA.Generator (for testing purposes)
- DocumentNumber.PaymentCardNumber.VISAElectron.Generator (for testing purposes)
- DocumentNumber.Portugal.BankAccountNumber.Generator (for testing purposes)
- DocumentNumber.Portugal.CitizenCard.Generator (for testing purposes)

## [1.4.1] 2022-06-28
### Fix
Yeah, I mess up a lib and this will fix it.

## [1.4.0] - 2022-06-27
### Added
- Portugal NIF Generator (for testing purposes)
- Portugal NISS Generator (for testing purposes)
- Portugal CC Generator (for testing purposes)

## [1.3.0] - 2021-10-23
### Added
- Portugal NIB Validator - Contribuition of [@joaomatossilva](https://github.com/joaomatossilva)
- International Bank Account Number (IBAN) Validator Contribuition of [@joaomatossilva](https://github.com/joaomatossilva)

## [1.2.0] - 2021-09-17
### Added
- DocumentNumber.PaymentCardNumber.AmericanExpress.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.PaymentCardNumber.Maestro.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.PaymentCardNumber.MaestroUK.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.PaymentCardNumber.Mastercard.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.PaymentCardNumber.VISA.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.PaymentCardNumber.VISAElectron.Validator (netcoreapp3.1; netstandard2.1)

## [1.1.0] - 2021-01-17
### Added
- DocumentNumber.Portugal.CitizenCard.Validator (netcoreapp3.1; netstandard2.1)
- DocumentNumber.ValidatorAbstractions (netstandard2.1)
- DocumentNumber.Portugal.Nif.Validator (netstandard2.1)
- DocumentNumber.Portugal.Niss.Validator (netstandard2.1)
## [1.0.0] - 2020-12-30
### Added
- DocumentNumber.ValidatorAbstractions (netcoreapp3.1)
- DocumentNumber.Portugal.Nif.Validator (netcoreapp3.1)
- DocumentNumber.Portugal.Niss.Validator (netcoreapp3.1)
