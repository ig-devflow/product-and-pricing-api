namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreListItemDto(
    int Id,
    string Name,
    string Code,
    bool IsActive,
    bool IsPhysicalCentre,
    string? City,
    string? CountryName,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);