namespace ProductsAndPricingNew.Domain.Entities.Common;

public sealed record Address
{
    private Address() { }

    private Address(
        int? countryId,
        string? street,
        string? district,
        string? city,
        string? postalCode)
    {
        CountryId = countryId;
        Street = Normalize(street);
        this.District = Normalize(district);
        City = Normalize(city);
        PostalCode = Normalize(postalCode);
    }

    public int? CountryId { get; init; }
    public string? Street { get; init; }
    public string? District { get; init; }
    public string? City { get; init; }
    public string? PostalCode { get; init; }

    public bool IsEmpty =>
        CountryId is null &&
        Street is null &&
        District is null &&
        City is null &&
        PostalCode is null;

    public static Address Empty { get; } = new(null, null, null, null, null);

    public static Address Create(
        int? countryId,
        string? street,
        string? district,
        string? city,
        string? postalCode)
    {
        Address address = new(countryId, street, district, city, postalCode);
        return address.IsEmpty ? Empty : address;
    }

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}