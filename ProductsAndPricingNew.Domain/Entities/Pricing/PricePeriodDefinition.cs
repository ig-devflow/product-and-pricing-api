using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Pricing;

public sealed record PricePeriodDefinition(
    DateRange Period,
    int CurrencyId,
    PriceBandingMethod BandingMethod,
    IReadOnlyCollection<PriceBandDefinition> Bands
);