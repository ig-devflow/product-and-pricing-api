namespace ProductsAndPricingNew.Application.Features.Division.Dtos;

public sealed record DivisionListItemDto(
    int Id,
    string Name,
    bool ShowInDropdown,
    bool IsActive
);