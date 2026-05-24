using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// Handler for working-week unit types: a five-day week that skips Saturdays and Sundays. Date
/// arithmetic always lands on, and is measured between, weekdays.
/// </summary>
public sealed class WorkingWeekUnitTypeHandler : IUnitTypeHandler
{
    private const int WorkingDaysPerWeek = 5;
    private const int Saturday = 6;
    private const int Sunday = 7;
    private const int Monday = 1;

    public UnitCalculationKind Kind => UnitCalculationKind.WorkingWeek;

    public void Normalize(ref int major, ref int minor)
    {
        while (minor < 0 && major > 0)
        {
            major -= 1;
            minor += WorkingDaysPerWeek;
        }

        if (minor >= WorkingDaysPerWeek)
        {
            major += minor / WorkingDaysPerWeek;
            minor %= WorkingDaysPerWeek;
        }
    }

    public DateOnly AddTo(DateOnly date, int major, int minor)
    {
        if (major == 0 && minor == 0)
            return date;

        date = MoveOffWeekend(date);
        Normalize(ref major, ref minor);

        DateOnly plusWeeks = date.AddDays(major * 7);
        DateOnly plusDays = plusWeeks.AddDays(minor);

        if (IsoDayOfWeek(plusDays) >= Saturday || IsoDayOfWeek(plusDays) < IsoDayOfWeek(plusWeeks))
            plusDays = plusDays.AddDays(2);

        return plusDays;
    }

    public DateOnly SubtractFrom(DateOnly date, int major, int minor)
    {
        if (major == 0 && minor == 0)
            return date;

        date = MoveOffWeekend(date);

        DateOnly result = date.AddDays(-(major * 7)).AddDays(-minor);

        return IsoDayOfWeek(result) is Saturday or Sunday
            ? result.AddDays(-2)
            : result;
    }

    public (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding)
    {
        (DateOnly start, DateOnly end) = NormalizeRange(range.Start, range.End);

        if (start == end)
            return (0, 0);

        int totalDays = end.DayNumber - start.DayNumber;
        int weekendSkip = IsoDayOfWeek(start) <= IsoDayOfWeek(end) ? 0 : 2;

        return (totalDays / 7, totalDays % 7 - weekendSkip);
    }

    public string Format(int major, int minor) => $"{major} Work Weeks, {minor} Days";

    private static (DateOnly start, DateOnly end) NormalizeRange(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new DomainException("A working-week range cannot start after it ends.");

        if (IsoDayOfWeek(start) >= Saturday)
            start = NextMonday(start);

        if (IsoDayOfWeek(end) is Sunday or Monday)
            end = PreviousSaturday(end);

        // Both endpoints fell on the same weekend — collapse to zero units.
        if (start >= end)
            end = start;

        return (start, end);
    }

    private static DateOnly MoveOffWeekend(DateOnly date) =>
        IsoDayOfWeek(date) >= Saturday ? NextMonday(date) : date;

    private static DateOnly NextMonday(DateOnly date)
    {
        do
        {
            date = date.AddDays(1);
        }
        while (IsoDayOfWeek(date) != Monday);

        return date;
    }

    private static DateOnly PreviousSaturday(DateOnly date)
    {
        do
        {
            date = date.AddDays(-1);
        }
        while (IsoDayOfWeek(date) != Saturday);

        return date;
    }

    // System.DayOfWeek is Sunday=0..Saturday=6; this maps to ISO Monday=1..Sunday=7.
    private static int IsoDayOfWeek(DateOnly date) => ((int)date.DayOfWeek + 6) % 7 + 1;
}
