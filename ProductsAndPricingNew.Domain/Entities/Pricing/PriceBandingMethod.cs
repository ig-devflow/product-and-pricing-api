namespace ProductsAndPricingNew.Domain.Entities.Pricing;

/// <summary>
/// How a price period's bands combine when pricing a quantity of units. This is a pricing-maths
/// choice that belongs to the period — it is unrelated to the offering's date schedule.
/// </summary>
public enum PriceBandingMethod
{
    /// <summary>The single band that matches the unit count prices the whole quantity.</summary>
    Discrete = 1,

    /// <summary>Each band prices the units that fall within its own range.</summary>
    Cumulative = 2,

    /// <summary>Bands stack on top of one another as the quantity grows.</summary>
    Compound = 3
}