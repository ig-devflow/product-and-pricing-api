namespace ProductsAndPricingNew.Application.Features.Package.Models;

public sealed record PackageListItemDto(
    int Id,
    string DivisionName,
    string Name,
    bool IsActive,
    string? Description,
    decimal Commission,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
