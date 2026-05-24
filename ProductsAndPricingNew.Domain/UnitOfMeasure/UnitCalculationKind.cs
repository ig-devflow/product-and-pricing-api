namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// The behaviour family of a <see cref="UnitType"/>. The unit-type set is reference data, but the
/// arithmetic each kind implies is code: a <see cref="IUnitTypeHandler"/> is registered per kind.
/// </summary>
public enum UnitCalculationKind
{
    Any = 0,
    FixedPrice = 1,
    Day = 2,
    CalendarWeek = 3,
    CalendarNightWeek = 4,
    WorkingWeek = 5,
    Month = 6
}