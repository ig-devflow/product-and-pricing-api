using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

/// <summary>
/// A supplement price for a pricing year and currency. When <see cref="PeriodStart"/> is null the
/// price applies to the whole year; otherwise it applies only within the given period.
/// Owned by <see cref="SupplementFeeOffering"/>.
/// </summary>
public sealed class SupplementPrice : Entity<int>
{
    public int Year { get; private set; }
    public int CurrencyId { get; private set; }
    public decimal PricePerMajorUnit { get; private set; }
    public decimal PricePerMinorUnit { get; private set; }
    public DateOnly? PeriodStart { get; private set; }
    public DateOnly? PeriodEnd { get; private set; }

    public bool IsAllDates => PeriodStart is null;

    private SupplementPrice() { }

    internal SupplementPrice(int year, int currencyId, decimal pricePerMajorUnit, decimal pricePerMinorUnit, DateOnly? periodStart, DateOnly? periodEnd)
    {
        Year = year;
        CurrencyId = Guard.PositiveId(currencyId, nameof(CurrencyId));
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;

        EnsurePeriodValid();
        WithPrice(pricePerMajorUnit, pricePerMinorUnit);
    }

    internal void WithPrice(decimal pricePerMajorUnit, decimal pricePerMinorUnit)
    {
        if (pricePerMajorUnit < 0m || pricePerMinorUnit < 0m)
            throw new DomainException("A supplement price cannot be negative.");

        PricePerMajorUnit = pricePerMajorUnit;
        PricePerMinorUnit = pricePerMinorUnit;
    }

    internal bool Matches(int year, int currencyId, DateOnly? periodStart, DateOnly? periodEnd) =>
        Year == year && CurrencyId == currencyId && PeriodStart == periodStart && PeriodEnd == periodEnd;

    private void EnsurePeriodValid()
    {
        if (PeriodStart.HasValue != PeriodEnd.HasValue)
            throw new DomainException("A supplement period must have both a start and an end, or neither.");

        if (PeriodStart.HasValue && PeriodEnd!.Value < PeriodStart.Value)
            throw new DomainException("A supplement period cannot end before it starts.");
    }
}