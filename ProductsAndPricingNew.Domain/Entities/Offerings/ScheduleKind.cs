namespace ProductsAndPricingNew.Domain.Entities.Offerings;

/// <summary>
/// How an offering is scheduled. <see cref="Continuous"/> and <see cref="FixedRange"/> replace the
/// legacy "offering dates" pseudo-strategy: a product with no real scheduling pattern (e.g. a
/// fixed-price transfer) is now a first-class case rather than a degenerate strategy.
/// </summary>
public enum ScheduleKind
{
    /// <summary>Always bookable from the start date, with no recurring pattern and no fixed end.</summary>
    Continuous = 1,

    /// <summary>Bookable across a single bounded date range.</summary>
    FixedRange = 2,

    /// <summary>Recurring weekly starts on specific weekdays.</summary>
    Recurrence = 3,

    /// <summary>A set of fixed-duration date blocks.</summary>
    FixedDuration = 4,

    /// <summary>A set of explicit start dates.</summary>
    SpecificStartDates = 5
}
