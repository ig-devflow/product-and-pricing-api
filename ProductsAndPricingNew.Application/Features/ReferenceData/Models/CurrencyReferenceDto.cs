namespace ProductsAndPricingNew.Application.Features.ReferenceData.Models;

public sealed record CurrencyReferenceDto(
    int Id,
    string IsoCode,
    string Name,
    string Symbol
);
