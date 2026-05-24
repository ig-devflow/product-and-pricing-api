using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.Offerings.Definitions;

namespace ProductsAndPricingNew.Domain.Entities.Offerings;

/// <summary>
/// One contiguous stretch of an offering's schedule. Segments are owned by <see cref="ProductOffering"/>,
/// which guarantees they neither overlap nor use a <see cref="ScheduleKind"/> illegal for the
/// product's unit type.
/// </summary>
public sealed class ScheduleSegment
{
    private static readonly DateOnly OpenEndedSentinel = DateOnly.MaxValue;

    private readonly List<ScheduleSegmentDate> _dates = new();

    public ScheduleKind Kind { get; private set; }
    public DateOnly From { get; private set; }
    public DateOnly? To { get; private set; }
    public byte RecurrenceWeekdayMask { get; private set; }

    public IReadOnlyCollection<ScheduleSegmentDate> Dates => _dates.AsReadOnly();

    private ScheduleSegment() { }

    internal ScheduleSegment(ScheduleSegmentDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        Kind = definition.Kind;
        From = definition.From;
        To = definition.To;

        if (To.HasValue && To.Value < From)
            throw new DomainException("A schedule segment cannot end before it starts.");

        ApplyKindData(definition);
    }

    public DateOnly? EffectiveEnd => Kind switch
    {
        ScheduleKind.FixedDuration when !To.HasValue => _dates.Max(d => d.End ?? d.Start),
        ScheduleKind.SpecificStartDates when !To.HasValue => _dates.Max(d => d.Start),
        _ => To
    };

    public bool IsOpenEnded => EffectiveEnd is null;

    public IReadOnlyCollection<DayOfWeek> RecurrenceWeekdays => FromMask(RecurrenceWeekdayMask);

    public bool Covers(DateOnly date) =>
        date >= From && date <= (EffectiveEnd ?? OpenEndedSentinel);

    public bool Overlaps(DateOnly from, DateOnly? to) =>
        From <= (to ?? OpenEndedSentinel) && from <= (EffectiveEnd ?? OpenEndedSentinel);

    internal void Close(DateOnly endDate)
    {
        if (endDate < From)
            throw new DomainException("A schedule segment cannot be closed before it starts.");

        To = endDate;
    }

    private void ApplyKindData(ScheduleSegmentDefinition definition)
    {
        switch (definition.Kind)
        {
            case ScheduleKind.Continuous:
                break;

            case ScheduleKind.FixedRange:
                if (!To.HasValue)
                    throw new DomainException("A fixed-range segment requires an end date.");
                break;

            case ScheduleKind.Recurrence:
                if (definition.RecurrenceWeekdays is null || definition.RecurrenceWeekdays.Count == 0)
                    throw new DomainException("A recurrence segment requires at least one weekday.");
                RecurrenceWeekdayMask = ToMask(definition.RecurrenceWeekdays);
                break;

            case ScheduleKind.FixedDuration:
                if (definition.FixedDurationBlocks is null || definition.FixedDurationBlocks.Count == 0)
                    throw new DomainException("A fixed-duration segment requires at least one block.");
                foreach (var block in definition.FixedDurationBlocks)
                    _dates.Add(new ScheduleSegmentDate(block.Start, block.End));
                break;

            case ScheduleKind.SpecificStartDates:
                if (definition.SpecificDates is null || definition.SpecificDates.Count == 0)
                    throw new DomainException("A specific-start-dates segment requires at least one date.");
                foreach (DateOnly date in definition.SpecificDates.Distinct())
                    _dates.Add(new ScheduleSegmentDate(date, null));
                break;

            default:
                throw new DomainException($"Unknown schedule kind: {definition.Kind}.");
        }
    }

    private static byte ToMask(IEnumerable<DayOfWeek> weekdays)
    {
        int mask = 0;
        foreach (DayOfWeek day in weekdays)
            mask |= 1 << (int)day;

        return (byte)mask;
    }

    private static IReadOnlyCollection<DayOfWeek> FromMask(byte mask)
    {
        var days = new List<DayOfWeek>();
        foreach (DayOfWeek day in Enum.GetValues<DayOfWeek>())
        {
            if ((mask & (1 << (int)day)) != 0)
                days.Add(day);
        }

        return days;
    }
}
