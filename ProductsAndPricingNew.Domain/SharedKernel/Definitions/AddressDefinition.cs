namespace ProductsAndPricingNew.Domain.SharedKernel.Definitions;

public sealed record AddressDefinition(
    int? CountryId,
    string? Street,
    string? District,
    string? City,
    string? PostalCode)
{
    public bool IsEmpty =>
        CountryId is null
        && string.IsNullOrWhiteSpace(Street)
        && string.IsNullOrWhiteSpace(District)
        && string.IsNullOrWhiteSpace(City)
        && string.IsNullOrWhiteSpace(PostalCode);

    public static AddressDefinition Empty { get; } = new(null, null, null, null, null);
}