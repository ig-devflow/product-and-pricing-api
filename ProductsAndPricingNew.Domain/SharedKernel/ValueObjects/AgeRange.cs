using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct AgeRange : IEmptyValueObject
{
    public int? Minimum { get; }
    public int? Maximum { get; }

    public static readonly AgeRange Open = new(null, null);
    public bool IsEmpty => Minimum is null && Maximum is null;

    private AgeRange(int? minimum, int? maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
    }

    public static AgeRange Create(int? min, int? max)
    {
        if (min is null && max is null)
            return Open;

        EnsureValid(min, max);

        return new AgeRange(min, max);
    }

    public static AgeRange Create(AgeRangeDefinition? definition) =>
        definition is null ? Open : Create(definition.From, definition.To);

    public bool Contains(int age) =>
        (!Minimum.HasValue || age >= Minimum.Value)
        && (!Maximum.HasValue || age <= Maximum.Value);

    private static void EnsureValid(int? min, int? max)
    {
        if (min is < Rules.MinAge)
            throw new DomainException("Minimum age cannot be negative.");

        if (max is < Rules.MinAge)
            throw new DomainException("Maximum age cannot be negative.");

        if (min > Rules.MaxAge)
            throw new DomainException($"Minimum age must not exceed {Rules.MaxAge}.");

        if (max > Rules.MaxAge)
            throw new DomainException($"Maximum age must not exceed {Rules.MaxAge}.");

        if (min.HasValue && max.HasValue && min > max)
            throw new DomainException("Minimum age must be less than or equal to maximum age.");
    }

    public static class Rules
    {
        public const int MinAge = 0;
        public const int MaxAge = 120;
    }
}