using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Builders;

internal sealed class AddressDtoBuilder
{
    private int? _countryId = 1;
    private string? _street = "Street";
    private string? _district;
    private string? _city = "City";
    private string? _postalCode = "10001";

    public AddressDtoBuilder WithCountryId(int? countryId)
    {
        _countryId = countryId;
        return this;
    }

    public AddressDtoBuilder WithStreet(string? street)
    {
        _street = street;
        return this;
    }

    public AddressDtoBuilder WithDistrict(string? district)
    {
        _district = district;
        return this;
    }

    public AddressDtoBuilder WithCity(string? city)
    {
        _city = city;
        return this;
    }

    public AddressDtoBuilder WithPostalCode(string? postalCode)
    {
        _postalCode = postalCode;
        return this;
    }

    public AddressDto Build()
        => new(_street, _district, _city, _postalCode, _countryId);
}
