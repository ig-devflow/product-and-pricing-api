using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.Entities.Pricing;

/// <summary>A price for a contiguous range of units. Owned by <see cref="PricePeriod"/>.</summary>
public sealed class PriceBand
{
    public int MinUnits { get; private set; }
    public int MaxUnits { get; private set; }
    public decimal PricePerUnit { get; private set; }

    private PriceBand() { }

    internal PriceBand(int minUnits, int maxUnits, decimal pricePerUnit)
    {
        if (minUnits < 0)
            throw new DomainException("A price band minimum cannot be negative.");

        if (maxUnits < minUnits)
            throw new DomainException("A price band maximum cannot be less than its minimum.");

        if (pricePerUnit < 0m)
            throw new DomainException("A price band price cannot be negative.");

        MinUnits = minUnits;
        MaxUnits = maxUnits;
        PricePerUnit = pricePerUnit;
    }

    internal bool Overlaps(PriceBand other) =>
        MinUnits <= other.MaxUnits && other.MinUnits <= MaxUnits;
}