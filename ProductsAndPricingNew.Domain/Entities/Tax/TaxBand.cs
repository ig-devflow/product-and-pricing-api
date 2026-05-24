using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Tax;

/// <summary>
/// One tax rate within a <see cref="TaxRegimen"/>. The legacy per-band <c>ICriteria</c> is
/// replaced by <see cref="BandRuleset"/>: the band applies to a product row when that ruleset
/// matches. Bands are evaluated in <see cref="SequenceKey"/> order and the first match wins.
/// </summary>
public sealed class TaxBand : Entity<int>
{
    public int TaxRegimenId { get; private set; }
    public string Name { get; private set; } = null!;
    public string TaxCode { get; private set; } = null!;
    public decimal Rate { get; private set; }
    public int SequenceKey { get; private set; }
    public RulesetRef BandRuleset { get; private set; } = RulesetRef.None;

    private TaxBand() { }

    // Id is database-generated; bands are identified within a regimen by their (unique) tax code.
    internal TaxBand(int taxRegimenId, string name, string taxCode, decimal rate, int sequenceKey)
    {
        TaxRegimenId = taxRegimenId;
        TaxCode = taxCode.AsRequiredDomainText(nameof(TaxCode), Rules.TaxCodeMaxLength);

        ChangeName(name);
        ChangeRate(rate);
        ChangeSequenceKey(sequenceKey);
    }

    internal void ChangeName(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    internal void ChangeRate(decimal rate)
    {
        if (rate is < 0m or > 1m)
            throw new DomainException("Tax rate must be between 0 and 1 inclusive.");

        Rate = rate;
    }

    internal void ChangeSequenceKey(int sequenceKey)
    {
        if (sequenceKey <= 0)
            throw new DomainException("SequenceKey must be greater than zero.");

        SequenceKey = sequenceKey;
    }

    internal void ChangeBandRuleset(RulesetRef ruleset) => BandRuleset = ruleset;

    public static class Rules
    {
        public const int NameMaxLength = 200;
        public const int TaxCodeMaxLength = 20;
    }
}
