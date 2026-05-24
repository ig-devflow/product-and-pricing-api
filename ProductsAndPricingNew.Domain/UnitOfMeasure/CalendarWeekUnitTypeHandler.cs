using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// Handler for the seven-day week kinds. Calendar weeks and calendar night-weeks share identical
/// arithmetic and differ only in how the minor unit is labelled (days vs. nights).
/// </summary>
public sealed class CalendarWeekUnitTypeHandler : IUnitTypeHandler
{
    private const int DaysPerWeek = 7;

    private readonly string _minorLabel;

    public UnitCalculationKind Kind { get; }

    public CalendarWeekUnitTypeHandler(UnitCalculationKind kind)
    {
        if (kind is not (UnitCalculationKind.CalendarWeek or UnitCalculationKind.CalendarNightWeek))
            throw new DomainException($"{kind} is not a calendar-week unit kind.");

        Kind = kind;
        _minorLabel = kind == UnitCalculationKind.CalendarNightWeek ? "Nights" : "Days";
    }

    public void Normalize(ref int major, ref int minor)
    {
        while (minor < 0 && major > 0)
        {
            major -= 1;
            minor += DaysPerWeek;
        }

        if (minor >= DaysPerWeek)
        {
            major += minor / DaysPerWeek;
            minor %= DaysPerWeek;
        }
    }

    public DateOnly AddTo(DateOnly date, int major, int minor) =>
        date.AddDays(major * DaysPerWeek + minor);

    public DateOnly SubtractFrom(DateOnly date, int major, int minor) =>
        date.AddDays(-(major * DaysPerWeek + minor));

    public (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding)
    {
        int days = range.End.DayNumber - range.Start.DayNumber;
        return (days / DaysPerWeek, days % DaysPerWeek);
    }

    public string Format(int major, int minor) => $"{major} Weeks, {minor} {_minorLabel}";
}
