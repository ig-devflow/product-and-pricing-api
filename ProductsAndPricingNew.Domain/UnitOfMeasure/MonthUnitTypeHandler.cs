using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>Handler for month-based unit types. Minor units are not used.</summary>
public sealed class MonthUnitTypeHandler : IUnitTypeHandler
{
    public UnitCalculationKind Kind => UnitCalculationKind.Month;

    public void Normalize(ref int major, ref int minor) => EnsureNoMinor(minor);

    public DateOnly AddTo(DateOnly date, int major, int minor)
    {
        EnsureNoMinor(minor);
        return date.AddMonths(major);
    }

    public DateOnly SubtractFrom(DateOnly date, int major, int minor)
    {
        EnsureNoMinor(minor);
        return date.AddMonths(-major);
    }

    public (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding)
    {
        int months = (range.End.Year - range.Start.Year) * 12 + (range.End.Month - range.Start.Month);

        DateOnly anchored = range.Start.AddMonths(months);
        if (anchored > range.End)
        {
            months -= 1;
            anchored = range.Start.AddMonths(months);
        }

        int remainderDays = range.End.DayNumber - anchored.DayNumber;
        if (remainderDays == 0)
            return (months, 0);

        return rounding switch
        {
            UnitsRoundingMode.RoundUp => (months + 1, 0),
            UnitsRoundingMode.RoundDown => (months, 0),
            _ => throw new DomainException("The date range does not span a whole number of months.")
        };
    }

    public string Format(int major, int minor)
    {
        EnsureNoMinor(minor);
        return $"{major} months";
    }

    private static void EnsureNoMinor(int minor)
    {
        if (minor != 0)
            throw new DomainException("A month unit type does not allow minor units.");
    }
}
