using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// Handler for the unit kinds that are not tied to a calendar — fixed price, the "any" wildcard
/// and the no-units kind. Date arithmetic is unsupported and throws.
/// </summary>
public sealed class NonDateBasedUnitTypeHandler : IUnitTypeHandler
{
    public UnitCalculationKind Kind { get; }

    public NonDateBasedUnitTypeHandler(UnitCalculationKind kind)
    {
        if (kind is not (UnitCalculationKind.FixedPrice or UnitCalculationKind.Any))
            throw new DomainException($"{kind} is a date-based unit kind.");

        Kind = kind;
    }

    public void Normalize(ref int major, ref int minor)
    {
        if (Kind == UnitCalculationKind.FixedPrice && minor != 0)
            throw new DomainException("A fixed-price unit type does not allow minor units.");
    }

    public DateOnly AddTo(DateOnly date, int major, int minor) => throw NotDateBased();

    public DateOnly SubtractFrom(DateOnly date, int major, int minor) => throw NotDateBased();

    public (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding) => throw NotDateBased();

    public string Format(int major, int minor) => Kind switch
    {
        UnitCalculationKind.FixedPrice => $"{major} {(major == 1 ? "item" : "items")}",
        UnitCalculationKind.Any => $"{major} major units and {minor} units",
        _ => "0"
    };

    private DomainException NotDateBased() =>
        new($"Unit kind {Kind} is not date-based; date arithmetic is not supported.");
}
