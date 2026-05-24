using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Pricing;

/// <summary>
/// Prices for one currency over one date range, expressed as unit bands. Owned by
/// <see cref="OfferingYearPricing"/>.
/// </summary>
public sealed class PricePeriod
{
    private readonly List<PriceBand> _bands = new();

    public DateOnly Start { get; private set; }
    public DateOnly End { get; private set; }
    public int CurrencyId { get; private set; }
    public PriceBandingMethod BandingMethod { get; private set; }

    public IReadOnlyCollection<PriceBand> Bands => _bands.AsReadOnly();

    public DateRange Range => DateRange.Create(Start, End);

    private PricePeriod() { }

    internal PricePeriod(PricePeriodDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (definition.Bands is null || definition.Bands.Count == 0)
            throw new DomainException("A price period must have at least one price band.");

        Start = definition.Period.Start;
        End = definition.Period.End;
        CurrencyId = Guard.PositiveId(definition.CurrencyId, nameof(CurrencyId));
        BandingMethod = definition.BandingMethod;

        foreach (PriceBandDefinition band in definition.Bands.OrderBy(b => b.MinUnits))
        {
            var priceBand = new PriceBand(band.MinUnits, band.MaxUnits, band.PricePerUnit);

            if (_bands.Any(existing => existing.Overlaps(priceBand)))
                throw new DomainException("Price bands within a period cannot overlap on units.");

            _bands.Add(priceBand);
        }
    }

    internal bool OverlapsSameCurrency(PricePeriod other) =>
        CurrencyId == other.CurrencyId && Range.Overlaps(other.Range);
}