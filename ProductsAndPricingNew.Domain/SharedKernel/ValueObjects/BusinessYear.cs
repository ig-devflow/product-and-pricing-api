using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

/// <summary>
/// Starts on the first Monday of a calendar year and ends on the
/// Sunday of the week of the last Monday of the same calendar year, so the whole year is
/// covered by full Monday-to-Sunday weeks with no gaps between consecutive years.
/// </summary>
public readonly record struct BusinessYear : IComparable<BusinessYear>
{
    public int Year { get; }

    private BusinessYear(int year)
    {
        if (year is < 1 or > 9999)
            throw new DomainException($"Business year {year} is out of the supported range.");

        Year = year;
    }

    public static BusinessYear Of(int year) => new(year);

    /// <summary>
    /// Resolves the business year that contains <paramref name="date"/>. Dates between
    /// January 1 and the first Monday of <paramref name="date"/>.Year (exclusive) belong to the
    /// previous business year, because that year's last week (ending Sunday) reaches into
    /// early January.
    /// </summary>
    public static BusinessYear For(DateOnly date)
    {
        DateOnly firstMonday = FirstMondayOf(date.Year);
        return date < firstMonday ? new BusinessYear(date.Year - 1) : new BusinessYear(date.Year);
    }

    public DateOnly Start => FirstMondayOf(Year);

    public DateOnly End => LastMondayOf(Year).AddDays(6);

    public bool Contains(DateOnly date) => date >= Start && date <= End;

    public bool Contains(DateRange range) => range.Start >= Start && range.End <= End;

    public BusinessYear Next() => new(Year + 1);

    public BusinessYear Previous() => new(Year - 1);

    public DateRange ToDateRange() => DateRange.Create(Start, End);

    public int CompareTo(BusinessYear other) => Year.CompareTo(other.Year);

    public override string ToString() => $"Business Year: {Year} [{Start:yyyy-MM-dd}..{End:yyyy-MM-dd}]";

    private static DateOnly FirstMondayOf(int year)
    {
        DateOnly jan1 = new(year, 1, 1);
        int delta = ((int)DayOfWeek.Monday - (int)jan1.DayOfWeek + 7) % 7;
        return jan1.AddDays(delta);
    }

    private static DateOnly LastMondayOf(int year)
    {
        DateOnly dec31 = new(year, 12, 31);
        int delta = ((int)dec31.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
        return dec31.AddDays(-delta);
    }
}
