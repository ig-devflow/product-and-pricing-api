namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct OfferingClosure : IEmptyValueObject
{
    public DateOnly? DateStrategiesClosureDate { get; init; }
    public DateOnly? DecommissionDate { get; init; }

    public static readonly OfferingClosure Open = new(null, null);
    public DateOnly? EffectiveClosureDate
    {
        get
        {
            if (DateStrategiesClosureDate is null)
                return DecommissionDate;

            if (DecommissionDate is null)
                return DateStrategiesClosureDate;

            return DateStrategiesClosureDate < DecommissionDate
                ? DateStrategiesClosureDate
                : DecommissionDate;
        }
    }

    public bool IsEmpty => DateStrategiesClosureDate is null && DecommissionDate is null;

    private OfferingClosure(DateOnly? dateStrategiesClosureDate, DateOnly? decommissionDate)
    {
        DateStrategiesClosureDate = dateStrategiesClosureDate;
        DecommissionDate = decommissionDate;
    }

    public static OfferingClosure Create(DateOnly? dateStrategiesClosureDate, DateOnly? decommissionDate)
    {
        if (dateStrategiesClosureDate is null && decommissionDate is null)
            return Open;

        return new OfferingClosure(dateStrategiesClosureDate, decommissionDate);
    }

    public bool IsClosedOn(DateOnly date)
    {
        var effective = EffectiveClosureDate;
        return effective.HasValue && date >= effective.Value;
    }

    public OfferingClosure WithStrategies(DateOnly? date) =>
        this with { DateStrategiesClosureDate = date };

    public OfferingClosure WithDecommission(DateOnly? date) =>
        this with { DecommissionDate = date };
}