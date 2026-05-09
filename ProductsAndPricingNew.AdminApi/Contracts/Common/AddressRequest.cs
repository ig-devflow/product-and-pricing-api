namespace ProductsAndPricingNew.AdminApi.Contracts.Common;

public sealed record AddressRequest(
    string? Street,
    string? District,
    string? City,
    string? PostalCode,
    int? CountryId
);