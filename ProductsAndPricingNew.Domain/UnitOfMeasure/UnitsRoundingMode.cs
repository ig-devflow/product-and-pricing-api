namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>How a unit count is resolved when a date range does not divide evenly into units.</summary>
public enum UnitsRoundingMode
{
    RoundUp = 1,
    RoundDown = 2,
    Error = 3
}