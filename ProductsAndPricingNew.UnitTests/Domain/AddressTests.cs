using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain;

public sealed class AddressTests
{
    [Fact]
    public void Create_WithEmptyInput_ReturnsEmpty()
    {
        Address address = Address.Create(null, " ", null, null, null);

        Assert.Same(Address.Empty, address);
    }

    [Fact]
    public void Create_WithPartialAddressWithoutCountryId_Throws()
    {
        Assert.Throws<DomainException>(() => Address.Create(null, "Street", null, null, null));
    }

    [Fact]
    public void Create_WithValidAddress_NormalizesStrings()
    {
        Address address = Address.Create(1, "  Main   Street  ", "  District  ", "  New   York  ", "  10001  ");

        Assert.Equal(1, address.CountryId);
        Assert.Equal("Main Street", address.Street);
        Assert.Equal("District", address.District);
        Assert.Equal("New York", address.City);
        Assert.Equal("10001", address.PostalCode);
    }

    [Fact]
    public void Create_WithTooLongField_Throws()
    {
        string tooLong = new('A', Address.Rules.AddressFieldMaxLength + 1);

        Assert.Throws<DomainException>(() => Address.Create(1, tooLong, null, null, null));
    }
}
