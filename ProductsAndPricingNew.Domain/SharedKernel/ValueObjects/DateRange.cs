using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct DateRange
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    private DateRange(DateOnly start, DateOnly end)
    {
        Start = start;
        End = end;
    }

    public static DateRange Create(DateOnly start, DateOnly end)
    {
        if (end < start)
            throw new DomainException("A date range cannot end before it starts.");

        return new DateRange(start, end);
    }

    public bool Contains(DateOnly date) => date >= Start && date <= End;

    public bool Contains(DateRange other) => other.Start >= Start && other.End <= End;

    public bool Overlaps(DateRange other) => Start <= other.End && other.Start <= End;
}
