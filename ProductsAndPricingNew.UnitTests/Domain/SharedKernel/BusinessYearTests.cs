using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.SharedKernel;

public sealed class BusinessYearTests
{
    [Theory]
    [InlineData(2024, 1, 1)]  // 2024-01-01 is Monday → first Monday is Jan 1
    [InlineData(2025, 1, 6)]  // 2025-01-01 is Wednesday → first Monday is Jan 6
    [InlineData(2026, 1, 5)]  // 2026-01-01 is Thursday → first Monday is Jan 5
    [InlineData(2027, 1, 4)]  // 2027-01-01 is Friday → first Monday is Jan 4
    public void Start_Is_FirstMonday_Of_CalendarYear(int year, int month, int day)
    {
        DateOnly start = BusinessYear.Of(year).Start;

        Assert.Equal(new DateOnly(year, month, day), start);
        Assert.Equal(DayOfWeek.Monday, start.DayOfWeek);
    }

    [Theory]
    [InlineData(2024, 2025, 1, 5)]  // last Monday 2024 = Dec 30 → end = Jan 5, 2025 (Sunday)
    [InlineData(2025, 2026, 1, 4)]  // last Monday 2025 = Dec 29 → end = Jan 4, 2026
    [InlineData(2027, 2028, 1, 2)]  // last Monday 2027 = Dec 27 → end = Jan 2, 2028
    public void End_Is_Sunday_Of_LastMonday_Week(int year, int endYear, int endMonth, int endDay)
    {
        DateOnly end = BusinessYear.Of(year).End;

        Assert.Equal(new DateOnly(endYear, endMonth, endDay), end);
        Assert.Equal(DayOfWeek.Sunday, end.DayOfWeek);
    }

    [Theory]
    [InlineData("2025-01-01", 2024)] // before first Monday of 2025 → belongs to BY2024
    [InlineData("2025-01-05", 2024)] // last day of BY2024 (Sunday)
    [InlineData("2025-01-06", 2025)] // first Monday of 2025 → starts BY2025
    [InlineData("2024-12-31", 2024)] // tail of 2024 still in BY2024
    [InlineData("2024-01-01", 2024)] // Monday → starts BY2024
    [InlineData("2027-01-03", 2026)] // BY2026 ends 2027-01-03
    [InlineData("2027-01-04", 2027)] // first Monday of 2027
    public void For_Maps_Date_To_Correct_BusinessYear(string isoDate, int expectedYear)
    {
        BusinessYear by = BusinessYear.For(DateOnly.Parse(isoDate));

        Assert.Equal(expectedYear, by.Year);
    }

    [Fact]
    public void Consecutive_Years_Have_No_Gap_And_No_Overlap()
    {
        BusinessYear by = BusinessYear.Of(2024);
        BusinessYear next = by.Next();

        Assert.Equal(by.End.AddDays(1), next.Start);
    }

    [Theory]
    [InlineData(2024)]
    [InlineData(2025)]
    [InlineData(2026)]
    [InlineData(2027)]
    public void Year_Covers_WholeNumberOfWeeks_FromMonday_To_Sunday(int year)
    {
        BusinessYear by = BusinessYear.Of(year);

        int totalDays = by.End.DayNumber - by.Start.DayNumber + 1;

        Assert.Equal(0, totalDays % 7);
        Assert.Equal(DayOfWeek.Monday, by.Start.DayOfWeek);
        Assert.Equal(DayOfWeek.Sunday, by.End.DayOfWeek);
    }

    [Fact]
    public void Contains_Date_Inside_Bounds_True()
    {
        BusinessYear by = BusinessYear.Of(2025);

        Assert.True(by.Contains(by.Start));
        Assert.True(by.Contains(by.End));
        Assert.True(by.Contains(new DateOnly(2025, 6, 1)));
    }

    [Fact]
    public void Contains_Date_Outside_Bounds_False()
    {
        BusinessYear by = BusinessYear.Of(2025);

        Assert.False(by.Contains(by.Start.AddDays(-1)));
        Assert.False(by.Contains(by.End.AddDays(1)));
    }

    [Fact]
    public void Contains_DateRange_Spilling_Into_Next_BusinessYear_False()
    {
        BusinessYear by = BusinessYear.Of(2025);
        DateRange spillsOver = DateRange.Create(by.End, by.End.AddDays(1));

        Assert.False(by.Contains(spillsOver));
    }

    [Fact]
    public void Contains_DateRange_Inside_Bounds_True()
    {
        BusinessYear by = BusinessYear.Of(2025);
        DateRange inside = DateRange.Create(new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 30));

        Assert.True(by.Contains(inside));
    }

    [Fact]
    public void ToDateRange_Equals_Start_To_End()
    {
        BusinessYear by = BusinessYear.Of(2025);
        DateRange range = by.ToDateRange();

        Assert.Equal(by.Start, range.Start);
        Assert.Equal(by.End, range.End);
    }

    [Fact]
    public void Previous_And_Next_Are_Symmetric()
    {
        BusinessYear by = BusinessYear.Of(2025);

        Assert.Equal(by, by.Next().Previous());
        Assert.Equal(by, by.Previous().Next());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(10000)]
    public void Of_OutOfRange_Throws(int year)
    {
        Assert.Throws<DomainException>(() => BusinessYear.Of(year));
    }
}
