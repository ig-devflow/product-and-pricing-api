using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.PricingRef;

public sealed class PricingYearTests
{
    [Fact]
    public void Create_SetsValues()
    {
        PricingYear year = PricingYear.Create(divisionId: 7, year: 2026);

        Assert.Equal(7, year.DivisionId);
        Assert.Equal(2026, year.Year);
        Assert.Equal(RulesetRef.None, year.EarlyBirdRuleset);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_InvalidDivision_Throws(int divisionId)
    {
        Assert.Throws<DomainException>(() => PricingYear.Create(divisionId, 2026));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Create_InvalidYear_Throws(int year)
    {
        Assert.Throws<DomainException>(() => PricingYear.Create(7, year));
    }

    [Fact]
    public void ChangeEarlyBirdRuleset_SetsRef()
    {
        PricingYear year = PricingYear.Create(7, 2026);

        year.ChangeEarlyBirdRuleset(RulesetRef.Create(42));

        Assert.Equal(RulesetRef.Create(42), year.EarlyBirdRuleset);
        Assert.True(year.EarlyBirdRuleset.IsSet);
    }
}