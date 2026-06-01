namespace ProductsAndPricingNew.Application.Features.AccountCategory.Models;

public sealed record AccountCategoryDetailsDto(
    int Id,
    int DivisionId,
    string Name,
    bool IsActive,
    string Version,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
