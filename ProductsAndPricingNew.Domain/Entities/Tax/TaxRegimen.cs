using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Tax;

/// <summary>
/// A set of tax bands that applies to a country or a single centre over a validity period.
/// At calculation time the pricing engine selects the regimen for a booking by scope and date,
/// then takes the first band (in <see cref="TaxBand.SequenceKey"/> order) whose ruleset matches
/// the product row.
/// </summary>
public sealed class TaxRegimen : AggregateRoot<int>
{
    private readonly List<TaxBand> _bands = new();

    public string Name { get; private set; } = null!;
    public TaxScope Scope { get; private set; }
    public DateOnly ValidFrom { get; private set; }
    public DateOnly? ValidTo { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<TaxBand> Bands => _bands.AsReadOnly();

    private TaxRegimen() { }

    public static TaxRegimen Create(string name, TaxScope scope, DateOnly validFrom, DateOnly? validTo)
    {
        TaxRegimen regimen = new()
        {
            Scope = scope,
            IsActive = true
        };

        regimen.Rename(name);
        regimen.ChangeValidity(validFrom, validTo);

        return regimen;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeScope(TaxScope scope) => Scope = scope;

    public void ChangeValidity(DateOnly validFrom, DateOnly? validTo)
    {
        if (validTo.HasValue && validTo.Value < validFrom)
            throw new DomainException("Tax regimen ValidTo cannot be earlier than ValidFrom.");

        ValidFrom = validFrom;
        ValidTo = validTo;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public bool IsValidOn(DateOnly date) =>
        date >= ValidFrom && (!ValidTo.HasValue || date <= ValidTo.Value);

    public TaxBand AddBand(string name, string taxCode, decimal rate, int sequenceKey)
    {
        TaxBand band = new(Id, name, taxCode, rate, sequenceKey);

        if (_bands.Any(existing => CodeEquals(existing.TaxCode, band.TaxCode)))
            throw new DomainException($"A band with tax code '{band.TaxCode}' already exists in this regimen.");

        _bands.Add(band);

        return band;
    }

    public void UpdateBand(string taxCode, string name, decimal rate, int sequenceKey)
    {
        TaxBand band = GetBand(taxCode);
        band.ChangeName(name);
        band.ChangeRate(rate);
        band.ChangeSequenceKey(sequenceKey);
    }

    public void ChangeBandRuleset(string taxCode, RulesetRef ruleset) =>
        GetBand(taxCode).ChangeBandRuleset(ruleset);

    public void RemoveBand(string taxCode) => _bands.Remove(GetBand(taxCode));

    private TaxBand GetBand(string taxCode)
    {
        string target = (taxCode ?? string.Empty).Trim();

        return _bands.FirstOrDefault(band => CodeEquals(band.TaxCode, target))
               ?? throw new DomainException($"Band '{taxCode}' was not found in regimen {Id}.");
    }

    private static bool CodeEquals(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    public static class Rules
    {
        public const int NameMaxLength = 200;
    }
}
