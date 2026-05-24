using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.Tax;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.Tax;

public sealed class TaxRegimenTests
{
    private static TaxRegimen NewRegimen() =>
        TaxRegimen.Create(
            "UK VAT",
            TaxScope.ForCountry(44),
            new DateOnly(2026, 1, 1),
            validTo: null);

    [Fact]
    public void Create_SetsDefaults()
    {
        TaxRegimen regimen = NewRegimen();

        Assert.Equal("UK VAT", regimen.Name);
        Assert.Equal(TaxScopeKind.Country, regimen.Scope.Kind);
        Assert.Equal(44, regimen.Scope.TargetId);
        Assert.True(regimen.IsActive);
        Assert.Empty(regimen.Bands);
    }

    [Fact]
    public void ForCentre_SetsCentreScope()
    {
        TaxScope scope = TaxScope.ForCentre(9);

        Assert.Equal(TaxScopeKind.Centre, scope.Kind);
        Assert.Equal(9, scope.TargetId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void TaxScope_InvalidTarget_Throws(int targetId)
    {
        Assert.Throws<DomainException>(() => TaxScope.ForCountry(targetId));
    }

    [Fact]
    public void ChangeValidity_EndBeforeStart_Throws()
    {
        TaxRegimen regimen = NewRegimen();

        Assert.Throws<DomainException>(() =>
            regimen.ChangeValidity(new DateOnly(2026, 6, 1), new DateOnly(2026, 1, 1)));
    }

    [Fact]
    public void IsValidOn_RespectsOpenEndedRange()
    {
        TaxRegimen regimen = NewRegimen();

        Assert.False(regimen.IsValidOn(new DateOnly(2025, 12, 31)));
        Assert.True(regimen.IsValidOn(new DateOnly(2026, 1, 1)));
        Assert.True(regimen.IsValidOn(new DateOnly(2099, 1, 1)));
    }

    [Fact]
    public void AddBand_AddsBand()
    {
        TaxRegimen regimen = NewRegimen();

        TaxBand band = regimen.AddBand("Standard", "20000", 0.2m, sequenceKey: 1);

        Assert.Single(regimen.Bands);
        Assert.Equal("20000", band.TaxCode);
        Assert.Equal(0.2m, band.Rate);
    }

    [Fact]
    public void AddBand_DuplicateTaxCode_Throws()
    {
        TaxRegimen regimen = NewRegimen();
        regimen.AddBand("Standard", "20000", 0.2m, 1);

        // Tax codes are matched case-insensitively.
        Assert.Throws<DomainException>(() => regimen.AddBand("Other", "20000", 0.1m, 2));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    public void AddBand_RateOutOfRange_Throws(decimal rate)
    {
        TaxRegimen regimen = NewRegimen();

        Assert.Throws<DomainException>(() => regimen.AddBand("Standard", "20000", rate, 1));
    }

    [Fact]
    public void AddBand_NonPositiveSequenceKey_Throws()
    {
        TaxRegimen regimen = NewRegimen();

        Assert.Throws<DomainException>(() => regimen.AddBand("Standard", "20000", 0.2m, sequenceKey: 0));
    }

    [Fact]
    public void UpdateBand_ChangesBand()
    {
        TaxRegimen regimen = NewRegimen();
        regimen.AddBand("Standard", "20000", 0.2m, 1);

        regimen.UpdateBand("20000", "Reduced", 0.05m, 3);

        TaxBand band = regimen.Bands.Single();
        Assert.Equal("Reduced", band.Name);
        Assert.Equal(0.05m, band.Rate);
        Assert.Equal(3, band.SequenceKey);
    }

    [Fact]
    public void ChangeBandRuleset_SetsRef()
    {
        TaxRegimen regimen = NewRegimen();
        regimen.AddBand("Standard", "20000", 0.2m, 1);

        regimen.ChangeBandRuleset("20000", RulesetRef.Create(7));

        Assert.Equal(RulesetRef.Create(7), regimen.Bands.Single().BandRuleset);
    }

    [Fact]
    public void RemoveBand_RemovesBand()
    {
        TaxRegimen regimen = NewRegimen();
        regimen.AddBand("Standard", "20000", 0.2m, 1);

        regimen.RemoveBand("20000");

        Assert.Empty(regimen.Bands);
    }

    [Fact]
    public void UpdateBand_NotFound_Throws()
    {
        TaxRegimen regimen = NewRegimen();

        Assert.Throws<DomainException>(() => regimen.UpdateBand("99999", "Ghost", 0.1m, 1));
    }
}
