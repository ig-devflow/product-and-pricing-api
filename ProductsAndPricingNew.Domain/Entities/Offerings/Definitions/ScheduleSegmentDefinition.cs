using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Offerings.Definitions;

/// <summary>
/// Application-to-domain input describing one schedule segment. Keeps the domain free of DTOs;
/// the factory methods build a well-formed definition for each <see cref="ScheduleKind"/>.
/// </summary>
public sealed class ScheduleSegmentDefinition
{
    public ScheduleKind Kind { get; }
    public DateOnly From { get; }
    public DateOnly? To { get; }
    public IReadOnlyCollection<DayOfWeek>? RecurrenceWeekdays { get; private set; }
    public IReadOnlyCollection<DateRange>? FixedDurationBlocks { get; private set; }
    public IReadOnlyCollection<DateOnly>? SpecificDates { get; private set; }

    // Base ctor sets only the fields every segment has; kind-specific data is assigned in factories.
    private ScheduleSegmentDefinition(ScheduleKind kind, DateOnly from, DateOnly? to)
    {
        Kind = kind;
        From = from;
        To = to;
    }

    public static ScheduleSegmentDefinition Continuous(DateOnly from, DateOnly? to = null)
    {
        return new ScheduleSegmentDefinition(ScheduleKind.Continuous, from, to);
    }

    public static ScheduleSegmentDefinition FixedRange(DateOnly from, DateOnly to)
    {
        return new ScheduleSegmentDefinition(ScheduleKind.FixedRange, from, to);
    }

    public static ScheduleSegmentDefinition Recurrence(DateOnly from, DateOnly? to, IReadOnlyCollection<DayOfWeek> weekdays)
    {
        return new ScheduleSegmentDefinition(ScheduleKind.Recurrence, from, to)
        {
            RecurrenceWeekdays = weekdays
        };
    }

    public static ScheduleSegmentDefinition FixedDuration(IReadOnlyCollection<DateRange> blocks)
    {
        if (blocks == null || blocks.Count == 0)
            throw new DomainException("A fixed-duration segment requires at least one block.");

        return new ScheduleSegmentDefinition(
            ScheduleKind.FixedDuration,
            blocks.Min(b => b.Start),
            blocks.Max(b => b.End))
        {
            FixedDurationBlocks = blocks
        };
    }

    public static ScheduleSegmentDefinition SpecificStartDates(IReadOnlyCollection<DateOnly> dates)
    {
        if (dates == null || dates.Count == 0)
            throw new DomainException("A specific-start-dates segment requires at least one date.");

        return new ScheduleSegmentDefinition(
            ScheduleKind.SpecificStartDates,
            dates.Min(),
            dates.Max())
        {
            SpecificDates = dates
        };
    }
}