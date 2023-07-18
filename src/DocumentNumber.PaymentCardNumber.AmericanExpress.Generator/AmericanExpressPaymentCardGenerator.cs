﻿namespace DocumentNumber.PaymentCardNumber.AmericanExpress.Generator
{
  using DocumentNumber.PaymentCardNumber.Generator.Common.Generators;

  public class AmericanExpressPaymentCardGenerator : CreditCardGeneratorBase
  {
    public AmericanExpressPaymentCardGenerator()
    {
      this.ValidStartNumbers = new[] { "34", "37" };
      this.ValidDocumentNumberLengths = new[] { 15 };
    }
  }
}