using ProductsAndPricingNew.Domain.Common.Extensions;

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
        Street = street.AsOptionalDomainText();
        District = district.AsOptionalDomainText();
        City = city.AsOptionalDomainText();
        PostalCode = postalCode.AsOptionalDomainText();
    }

    public int? CountryId { get; }
    public string? Street { get; }
    public string? District { get; }
    public string? City { get; }
    public string? PostalCode { get; }

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
}