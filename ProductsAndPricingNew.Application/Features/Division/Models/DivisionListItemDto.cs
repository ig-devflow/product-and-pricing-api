namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionListItemDto(
    int Id,
    string Name,
    bool IsActive,
    string? WebsiteUrl,
    string? HeadOfficeEmail,
    string? City,
    string? CountryName,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);