using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Discounts;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.FeeOrDiscount;

public sealed class DiscountTests
{
    private static Discount NewDiscount() =>
        Discount.Create("Summer offer", divisionId: 7, DiscountType.SpecialOffer);

    [Fact]
    public void Create_SetsDefaults()
    {
        Discount discount = NewDiscount();

        Assert.Equal("Summer offer", discount.Name);
        Assert.Equal(7, discount.DivisionId);
        Assert.Equal(DiscountType.SpecialOffer, discount.Type);
        Assert.True(discount.IsActive);
        Assert.Equal(RulesetRef.None, discount.ShouldApplyRuleset);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2)]
    public void Create_InvalidDivision_Throws(int divisionId)
    {
        Assert.Throws<DomainException>(() =>
            Discount.Create("Summer offer", divisionId, DiscountType.MarketDiscount));
    }

    [Fact]
    public void ChangeShouldApplyRuleset_SetsRef()
    {
        Discount discount = NewDiscount();

        discount.ChangeShouldApplyRuleset(RulesetRef.Create(13));

        Assert.Equal(RulesetRef.Create(13), discount.ShouldApplyRuleset);
    }

    [Fact]
    public void ChangeDescription_BlankBecomesNull()
    {
        Discount discount = NewDiscount();

        discount.ChangeDescription("   ");

        Assert.Null(discount.Description);
    }

    [Fact]
    public void ChangeDescription_TrimsValue()
    {
        Discount discount = NewDiscount();

        discount.ChangeDescription("  10% off courses  ");

        Assert.Equal("10% off courses", discount.Description);
    }

    [Fact]
    public void Deactivate_ClearsIsActive()
    {
        Discount discount = NewDiscount();

        discount.Deactivate();

        Assert.False(discount.IsActive);
    }
}
