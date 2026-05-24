namespace ProductsAndPricingNew.Application.Features.ReferenceData.Models;

public sealed record UnitTypeDto(
    int Id,
    string Name,
    bool IsDateBased,
    string CalculationKind
);