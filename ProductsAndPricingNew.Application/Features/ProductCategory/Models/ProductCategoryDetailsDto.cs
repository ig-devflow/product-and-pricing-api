namespace ProductsAndPricingNew.Application.Features.ProductCategory.Models;

public sealed record ProductCategoryDetailsDto(
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
