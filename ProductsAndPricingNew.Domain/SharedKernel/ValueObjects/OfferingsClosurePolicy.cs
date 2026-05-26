using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct OfferingsClosurePolicy : IEmptyValueObject
{
    public DateOnly? Value { get; }

    private OfferingsClosurePolicy(DateOnly? value)
    {
        Value = value;
    }

    public static readonly OfferingsClosurePolicy Open = new(null);
    public bool IsEmpty => Value == null;

    public bool BlocksNewOfferingsOn(DateOnly date) => Value.HasValue && date >= Value.Value;

    public static bool IsValid(DateOnly? date) =>
        !date.HasValue || date.Value >= DateOnly.FromDateTime(DateTime.Today);

    public static OfferingsClosurePolicy Create(DateOnly? offeringsClosureDate)
    {
        if (offeringsClosureDate is null)
            return Open;

        if (offeringsClosureDate.Value < DateOnly.FromDateTime(DateTime.Today))
            throw new DomainException("Closure policy dates must be in the future.");

        return new OfferingsClosurePolicy(offeringsClosureDate);
    }
}