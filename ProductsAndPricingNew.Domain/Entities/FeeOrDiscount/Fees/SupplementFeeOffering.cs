using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public sealed class SupplementFeeOffering : FeeOffering
{
    private readonly List<SupplementPrice> _prices = new();

    public bool HasSpecificDates { get; private set; }
    public int? MaximumUnits { get; private set; }

    public IReadOnlyCollection<SupplementPrice> Prices => _prices.AsReadOnly();

    public override FeePricingKind Kind => FeePricingKind.Supplement;

    private SupplementFeeOffering() { }

    private SupplementFeeOffering(int schoolId, int feeId, ActivePricingYears years)
        : base(schoolId, feeId, years)
    {
    }

    public static SupplementFeeOffering Create(int schoolId, Fee fee, UnitType feeUnitType, ActivePricingYears years)
    {
        ArgumentNullException.ThrowIfNull(fee);
        ArgumentNullException.ThrowIfNull(feeUnitType);

        if (feeUnitType.Id != fee.UnitTypeId)
            throw new DomainException("The supplied unit type does not match the fee's unit type.");

        if (!feeUnitType.IsDateBased)
            throw new DomainException("A supplement fee offering requires a fee with a date-based unit type.");

        return new SupplementFeeOffering(schoolId, fee.Id, years);
    }

    public void WithMaximumUnits(int? maximumUnits)
    {
        if (maximumUnits is < 1)
            throw new DomainException("Maximum units must be at least 1.");

        MaximumUnits = maximumUnits;
    }

    public void WithPrices(bool hasSpecificDates, IEnumerable<SupplementPriceDefinition> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);

        List<SupplementPriceDefinition> incoming = prices.ToList();

        foreach (SupplementPriceDefinition def in incoming)
        {
            if (!Years.Includes(def.Year))
                throw new DomainException($"Pricing year {def.Year} is outside the offering's active years.");

            Guard.PositiveId(def.CurrencyId, nameof(def.CurrencyId));

            bool defIsSpecific = def.PeriodStart.HasValue;
            if (hasSpecificDates && !defIsSpecific)
                throw new DomainException("All prices must have a specific period when HasSpecificDates is true.");
            if (!hasSpecificDates && defIsSpecific)
                throw new DomainException("Prices must not have a period when HasSpecificDates is false.");
        }

        HasSpecificDates = hasSpecificDates;

        var incomingKeys = incoming
            .Select(d => (d.Year, d.CurrencyId, d.PeriodStart, d.PeriodEnd))
            .ToHashSet();

        _prices.RemoveAll(p => !incomingKeys.Contains((p.Year, p.CurrencyId, p.PeriodStart, p.PeriodEnd)));

        foreach (SupplementPriceDefinition def in incoming)
        {
            SupplementPrice? existing = _prices.SingleOrDefault(p =>
                p.Matches(def.Year, def.CurrencyId, def.PeriodStart, def.PeriodEnd));

            if (existing is null)
                _prices.Add(new SupplementPrice(def.Year, def.CurrencyId, def.PricePerMajorUnit, def.PricePerMinorUnit, def.PeriodStart, def.PeriodEnd));
            else
                existing.WithPrice(def.PricePerMajorUnit, def.PricePerMinorUnit);
        }
    }
}
