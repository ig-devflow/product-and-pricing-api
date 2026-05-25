namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees.Definitions;

public sealed record OneOffFeePriceDefinition(
    int Year,
    int CurrencyId,
    decimal Amount
);