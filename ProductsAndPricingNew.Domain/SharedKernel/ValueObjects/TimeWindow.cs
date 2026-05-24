using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct TimeWindow(TimeOnly? From, TimeOnly? To)
{
    public static readonly TimeWindow Undefined = new(null, null);

    public bool HasValue => From.HasValue;

    public static TimeWindow Create(TimeOnly? from, TimeOnly? to)
    {
        if (from is null && to is null)
            return Undefined;

        if (from.HasValue != to.HasValue)
            throw new DomainException("TimeWindow From and To must both be set or both be null.");

        if (from!.Value > to!.Value)
            throw new DomainException("TimeWindow From cannot be later than To.");

        return new TimeWindow(from, to);
    }

    public static TimeWindow Create(TimeWindowDefinition? definition) =>
        definition is null ? Undefined : Create(definition.From, definition.To);
}
