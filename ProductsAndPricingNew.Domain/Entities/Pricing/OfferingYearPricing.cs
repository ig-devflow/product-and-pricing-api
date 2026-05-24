using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.Offerings;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Pricing;

/// <summary>
/// One pricing year for a single <see cref="ProductOffering"/>. Kept as its own aggregate because
/// years are edited and published independently — but a price period can only be added when the
/// owning offering's schedule actually covers it, which keeps prices and schedule from drifting
/// apart (the core flaw of the legacy model).
/// </summary>
public sealed class OfferingYearPricing : AggregateRoot<int>
{
    private readonly List<PricePeriod> _periods = new();

    public int ProductOfferingId { get; private set; }
    public int Year { get; private set; }

    public IReadOnlyCollection<PricePeriod> Periods => _periods.AsReadOnly();

    private OfferingYearPricing() { }

    public static OfferingYearPricing Create(int productOfferingId, int year)
    {
        if (year <= 0)
            throw new DomainException("Pricing year must be greater than zero.");

        return new OfferingYearPricing
        {
            ProductOfferingId = Guard.PositiveId(productOfferingId, nameof(ProductOfferingId)),
            Year = year
        };
    }

    /// <summary>
    /// Adds a price period. The owning <paramref name="offering"/> must be supplied so the period
    /// can be validated against its schedule — prices outside the schedule are rejected.
    /// </summary>
    public void AddPeriod(PricePeriodDefinition definition, ProductOffering offering)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(offering);

        if (offering.Id != ProductOfferingId)
            throw new DomainException("The supplied offering does not own this pricing record.");

        if (!offering.ScheduleCovers(definition.Period))
            throw new DomainException("The price period is not covered by the offering's schedule.");

        var period = new PricePeriod(definition);

        if (_periods.Any(existing => existing.OverlapsSameCurrency(period)))
            throw new DomainException("Price periods for the same currency cannot overlap on dates.");

        _periods.Add(period);
    }

    public void RemovePeriod(DateRange range, int currencyId)
    {
        PricePeriod period = _periods.FirstOrDefault(p => p.CurrencyId == currencyId && p.Range == range)
            ?? throw new DomainException("No matching price period was found.");

        _periods.Remove(period);
    }
}
