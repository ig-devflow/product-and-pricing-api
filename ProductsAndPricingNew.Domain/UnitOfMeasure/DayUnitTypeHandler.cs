using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>Handler for day-based unit types. Minor units are not used.</summary>
public sealed class DayUnitTypeHandler : IUnitTypeHandler
{
    public UnitCalculationKind Kind => UnitCalculationKind.Day;

    public void Normalize(ref int major, ref int minor) => EnsureNoMinor(minor);

    public DateOnly AddTo(DateOnly date, int major, int minor)
    {
        EnsureNoMinor(minor);
        return date.AddDays(major);
    }

    public DateOnly SubtractFrom(DateOnly date, int major, int minor)
    {
        EnsureNoMinor(minor);
        return date.AddDays(-major);
    }

    public (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding) =>
        (range.End.DayNumber - range.Start.DayNumber, 0);

    public string Format(int major, int minor)
    {
        EnsureNoMinor(minor);
        return $"{major} days";
    }

    private static void EnsureNoMinor(int minor)
    {
        if (minor != 0)
            throw new DomainException("A day unit type does not allow minor units.");
    }
}
