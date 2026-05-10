using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.SharedKernel;

public sealed class AddressTests
{
    [Fact]
    public void Create_WithAllNullOrWhitespace_ReturnsEmpty()
    {
        Address address = Address.Create(null, " ", "\t", null, "");

        Assert.Same(Address.Empty, address);
    }

    [Fact]
    public void Create_WithCountryOnly_ReturnsAddress()
    {
        Address address = Address.Create(1, null, null, null, null);

        Assert.Equal(1, address.CountryId);
        Assert.False(address.IsEmpty);
    }

    [Fact]
    public void Create_WithStreetButNoCountry_Throws()
    {
        Assert.Throws<DomainException>(() => Address.Create(null, "Street", null, null, null));
    }

    [Fact]
    public void Create_WithCountryIdZeroAndStreet_Throws()
    {
        Assert.Throws<DomainException>(() => Address.Create(0, "Street", null, null, null));
    }

    [Fact]
    public void Create_WithTooLongStreet_Throws()
    {
        string tooLong = new('A', Address.Rules.AddressFieldMaxLength + 1);

        Assert.Throws<DomainException>(() => Address.Create(1, tooLong, null, null, null));
    }

    [Fact]
    public void Create_NormalizesWhitespace()
    {
        Address address = Address.Create(1, "  Main   Street  ", "  North   District  ", "  New   York  ", "  10001  ");

        Assert.Equal("Main Street", address.Street);
        Assert.Equal("North District", address.District);
        Assert.Equal("New York", address.City);
        Assert.Equal("10001", address.PostalCode);
    }

    [Fact]
    public void EmptyAddress_IsEmpty()
    {
        Assert.True(Address.Empty.IsEmpty);
    }
}
