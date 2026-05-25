using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct TimeWindow
{
    public TimeOnly? From { get; }
    public TimeOnly? To { get; }

    private TimeWindow(TimeOnly? from, TimeOnly? to)
    {
        From = from;
        To = to;
    }

    public static readonly TimeWindow Undefined = new(null, null);
    public bool HasValue => From.HasValue;

    internal static TimeWindow Create(TimeOnly? from, TimeOnly? to)
    {
        if (from is null && to is null)
            return Undefined;

        if (from.HasValue != to.HasValue)
            throw new DomainException("TimeWindow From and To must both be set or both be null.");

        if (from!.Value > to!.Value)
            throw new DomainException("TimeWindow From cannot be later than To.");

        return new TimeWindow(from, to);
    }
}
