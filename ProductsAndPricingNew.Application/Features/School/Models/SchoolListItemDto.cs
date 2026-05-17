namespace ProductsAndPricingNew.Application.Features.School.Models;

public sealed record SchoolListItemDto(
    int Id,
    string Name,
    string LegacyCode,
    string CentreName,
    bool IsActive,
    string? City,
    string? CountryName,
    DateOnly? DecommissionDate,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);