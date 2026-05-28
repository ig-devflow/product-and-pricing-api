using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.AddOn.Models;

public sealed record AddOnListItemDto(
    int Id,
    string DivisionName,
    string Name,
    bool IsActive,
    AddOnType AddOnType,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
