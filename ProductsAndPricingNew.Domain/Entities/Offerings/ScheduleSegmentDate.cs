namespace ProductsAndPricingNew.Domain.Entities.Offerings;

/// <summary>
/// A dated entry inside a <see cref="ScheduleSegment"/>: a fixed-duration block (<see cref="End"/>
/// set) or a single explicit start date (<see cref="End"/> null). Owned by the segment.
/// </summary>
public sealed class ScheduleSegmentDate
{
    public DateOnly Start { get; private set; }
    public DateOnly? End { get; private set; }

    private ScheduleSegmentDate() { }

    internal ScheduleSegmentDate(DateOnly start, DateOnly? end)
    {
        Start = start;
        End = end;
    }
}