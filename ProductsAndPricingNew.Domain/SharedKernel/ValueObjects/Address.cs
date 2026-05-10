using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public sealed record Address
{
    public int? CountryId { get; }
    public string? Street { get; }
    public string? District { get; }
    public string? City { get; }
    public string? PostalCode { get; }

    private Address()
    {
    }

    private Address(
        int? countryId,
        string? street,
        string? district,
        string? city,
        string? postalCode)
    {
        CountryId = countryId;
        Street = street.AsOptionalDomainText(nameof(Street), Rules.AddressFieldMaxLength);
        District = district.AsOptionalDomainText(nameof(District), Rules.AddressFieldMaxLength);
        City = city.AsOptionalDomainText(nameof(City), Rules.AddressFieldMaxLength);
        PostalCode = postalCode.AsOptionalDomainText(nameof(PostalCode), Rules.AddressFieldMaxLength);
    }

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

        if (address.IsEmpty)
            return Empty;

        if (address.CountryId is null or <= 0)
            throw new DomainException("CountryId is required when address is provided.");

        return address;
    }

    public static class Rules
    {
        public const int AddressFieldMaxLength = 50;
    }
}
