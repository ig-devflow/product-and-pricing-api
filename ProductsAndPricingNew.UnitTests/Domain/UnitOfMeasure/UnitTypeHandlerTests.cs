using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.UnitTests.Domain.UnitOfMeasure;

public sealed class DayUnitTypeHandlerTests
{
    private readonly DayUnitTypeHandler _handler = new();

    [Fact]
    public void Kind_IsDay() => Assert.Equal(UnitCalculationKind.Day, _handler.Kind);

    [Fact]
    public void Normalize_RejectsMinor()
    {
        int major = 1, minor = 1;
        Assert.Throws<DomainException>(() => _handler.Normalize(ref major, ref minor));
    }

    [Fact]
    public void AddTo_AddsDays() =>
        Assert.Equal(new DateOnly(2026, 1, 10), _handler.AddTo(new DateOnly(2026, 1, 1), 9, 0));

    [Fact]
    public void SubtractFrom_SubtractsDays() =>
        Assert.Equal(new DateOnly(2026, 1, 1), _handler.SubtractFrom(new DateOnly(2026, 1, 10), 9, 0));

    [Fact]
    public void Between_CountsDays()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 8));

        Assert.Equal((7, 0), _handler.Between(range, UnitsRoundingMode.RoundDown));
    }

    [Fact]
    public void Format_RendersDays() => Assert.Equal("3 days", _handler.Format(3, 0));
}

public sealed class CalendarWeekUnitTypeHandlerTests
{
    private readonly CalendarWeekUnitTypeHandler _calendar = new(UnitCalculationKind.CalendarWeek);
    private readonly CalendarWeekUnitTypeHandler _night = new(UnitCalculationKind.CalendarNightWeek);

    [Fact]
    public void Ctor_RejectsNonWeekKind() =>
        Assert.Throws<DomainException>(() => new CalendarWeekUnitTypeHandler(UnitCalculationKind.Day));

    [Fact]
    public void Normalize_CarriesDaysIntoWeeks()
    {
        int major = 1, minor = 8;
        _calendar.Normalize(ref major, ref minor);

        Assert.Equal((2, 1), (major, minor));
    }

    [Fact]
    public void Normalize_BorrowsFromMajorWhenMinorIsNegative()
    {
        int major = 2, minor = -1;
        _calendar.Normalize(ref major, ref minor);

        Assert.Equal((1, 6), (major, minor));
    }

    [Fact]
    public void AddTo_AddsWeeksAndDays() =>
        Assert.Equal(
            new DateOnly(2026, 1, 16),
            _calendar.AddTo(new DateOnly(2026, 1, 1), 2, 1));

    [Fact]
    public void Between_ReturnsWeeksAndDays()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 1), new DateOnly(2026, 1, 17));

        Assert.Equal((2, 2), _calendar.Between(range, UnitsRoundingMode.RoundDown));
    }

    [Fact]
    public void Format_UsesDaysLabelForCalendarWeek() =>
        Assert.Equal("3 Weeks, 2 Days", _calendar.Format(3, 2));

    [Fact]
    public void Format_UsesNightsLabelForCalendarNightWeek() =>
        Assert.Equal("3 Weeks, 2 Nights", _night.Format(3, 2));
}

public sealed class WorkingWeekUnitTypeHandlerTests
{
    private readonly WorkingWeekUnitTypeHandler _handler = new();

    [Fact]
    public void Kind_IsWorkingWeek() => Assert.Equal(UnitCalculationKind.WorkingWeek, _handler.Kind);

    [Fact]
    public void Normalize_CarriesDaysWithFiveDayWeek()
    {
        int major = 1, minor = 6;
        _handler.Normalize(ref major, ref minor);

        Assert.Equal((2, 1), (major, minor));
    }

    [Fact]
    public void AddTo_OneWeekFromMonday_LandsOnMonday()
    {
        // 2026-01-05 is a Monday; one working week later is the next Monday.
        Assert.Equal(
            new DateOnly(2026, 1, 12),
            _handler.AddTo(new DateOnly(2026, 1, 5), 1, 0));
    }

    [Fact]
    public void AddTo_FromMondayPlusTwoDays_LandsOnWednesday()
    {
        Assert.Equal(
            new DateOnly(2026, 1, 7),
            _handler.AddTo(new DateOnly(2026, 1, 5), 0, 2));
    }

    [Fact]
    public void AddTo_FromMondayPlusFourDays_SkipsWeekendToTuesday()
    {
        // Mon + 4 working days = Friday; +1 more (minor==5 normalizes to 1 week 0 days)? No: 4 stays.
        // Mon + 4 days = Friday (no weekend skip needed).
        Assert.Equal(
            new DateOnly(2026, 1, 9),
            _handler.AddTo(new DateOnly(2026, 1, 5), 0, 4));
    }

    [Fact]
    public void AddTo_FromWeekend_MovesToMondayFirst()
    {
        // Saturday 2026-01-03 + 1 working week = following Monday + 1 week = 2026-01-12.
        Assert.Equal(
            new DateOnly(2026, 1, 12),
            _handler.AddTo(new DateOnly(2026, 1, 3), 1, 0));
    }
}

public sealed class MonthUnitTypeHandlerTests
{
    private readonly MonthUnitTypeHandler _handler = new();

    [Fact]
    public void Normalize_RejectsMinor()
    {
        int major = 1, minor = 1;
        Assert.Throws<DomainException>(() => _handler.Normalize(ref major, ref minor));
    }

    [Fact]
    public void AddTo_AddsMonths() =>
        Assert.Equal(
            new DateOnly(2026, 4, 15),
            _handler.AddTo(new DateOnly(2026, 1, 15), 3, 0));

    [Fact]
    public void Between_WholeMonths_ReturnsExact()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 15), new DateOnly(2026, 4, 15));

        Assert.Equal((3, 0), _handler.Between(range, UnitsRoundingMode.RoundDown));
    }

    [Fact]
    public void Between_PartialMonths_RoundsDown()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 15), new DateOnly(2026, 4, 20));

        Assert.Equal((3, 0), _handler.Between(range, UnitsRoundingMode.RoundDown));
    }

    [Fact]
    public void Between_PartialMonths_RoundsUp()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 15), new DateOnly(2026, 4, 20));

        Assert.Equal((4, 0), _handler.Between(range, UnitsRoundingMode.RoundUp));
    }

    [Fact]
    public void Between_PartialMonths_ErrorThrows()
    {
        DateRange range = DateRange.Create(new DateOnly(2026, 1, 15), new DateOnly(2026, 4, 20));

        Assert.Throws<DomainException>(() => _handler.Between(range, UnitsRoundingMode.Error));
    }
}

public sealed class NonDateBasedUnitTypeHandlerTests
{
    [Fact]
    public void Ctor_RejectsDateBasedKind() =>
        Assert.Throws<DomainException>(() => new NonDateBasedUnitTypeHandler(UnitCalculationKind.Day));

    [Fact]
    public void FixedPrice_RejectsMinor()
    {
        NonDateBasedUnitTypeHandler handler = new(UnitCalculationKind.FixedPrice);
        int major = 1, minor = 1;

        Assert.Throws<DomainException>(() => handler.Normalize(ref major, ref minor));
    }

    [Fact]
    public void FixedPrice_Format_UsesItemSingularAndPlural()
    {
        NonDateBasedUnitTypeHandler handler = new(UnitCalculationKind.FixedPrice);

        Assert.Equal("1 item", handler.Format(1, 0));
        Assert.Equal("3 items", handler.Format(3, 0));
    }

    [Fact]
    public void AddTo_AlwaysThrows()
    {
        NonDateBasedUnitTypeHandler handler = new(UnitCalculationKind.FixedPrice);

        Assert.Throws<DomainException>(() => handler.AddTo(new DateOnly(2026, 1, 1), 1, 0));
    }
}

public sealed class UnitTypeHandlerRegistryTests
{
    private readonly UnitTypeHandlerRegistry _registry = new();

    [Theory]
    [InlineData(UnitCalculationKind.Day)]
    [InlineData(UnitCalculationKind.CalendarWeek)]
    [InlineData(UnitCalculationKind.CalendarNightWeek)]
    [InlineData(UnitCalculationKind.WorkingWeek)]
    [InlineData(UnitCalculationKind.Month)]
    [InlineData(UnitCalculationKind.FixedPrice)]
    [InlineData(UnitCalculationKind.Any)]
    public void Resolve_ReturnsHandlerWithMatchingKind(UnitCalculationKind kind)
    {
        IUnitTypeHandler handler = _registry.Resolve(kind);

        Assert.Equal(kind, handler.Kind);
    }
}
