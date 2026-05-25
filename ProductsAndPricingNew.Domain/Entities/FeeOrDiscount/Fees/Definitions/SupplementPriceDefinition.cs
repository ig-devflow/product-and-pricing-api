namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees.Definitions;

public sealed record SupplementPriceDefinition(
    int Year,
    int CurrencyId,
    decimal PricePerMajorUnit,
    decimal PricePerMinorUnit,
    DateOnly? PeriodStart,
    DateOnly? PeriodEnd
);