namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record AddressDto(
    string? Street,
    string? District,
    string? City,
    string? PostalCode,
    int? CountryId
);