namespace ProductsAndPricingNew.Application.Features.Division.Dtos;

public sealed record DivisionAddressDto(
    string? Line1,
    string? Line2,
    string? Line3,
    string? Line4,
    int? CountryId
);