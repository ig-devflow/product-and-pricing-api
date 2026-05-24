using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct Percentage : IEmptyValueObject
{
    public decimal Value { get; }

    public static Percentage Zero => new(0m);
    public bool IsEmpty => Value == 0m;

    private Percentage(decimal value)
    {
        Value = value;
    }

    public static Percentage Create(decimal? value)
    {
        if (!value.HasValue)
            return Zero;

        if (value.Value <= 0m || value.Value > 100m)
            throw new DomainException("Percentage must be greater than 0 and at most 100.");

        return new Percentage(value.Value);
    }
}