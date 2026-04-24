namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionListItemDto(
    int Id,
    string Name,
    bool IsActive
);