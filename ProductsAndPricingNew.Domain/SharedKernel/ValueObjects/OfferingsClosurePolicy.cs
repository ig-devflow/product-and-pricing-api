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

    public static OfferingsClosurePolicy Create(DateOnly? offeringsClosureDate)
    {
        if (offeringsClosureDate is null)
            return Open;

        return new OfferingsClosurePolicy(offeringsClosureDate);
    }
}