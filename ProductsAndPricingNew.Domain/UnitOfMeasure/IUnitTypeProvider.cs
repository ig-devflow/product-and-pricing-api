namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// Read access to the unit-type reference data. Implemented over a cache so the unit-type lookups
/// done while pricing or validating products do not hit the database each time.
/// </summary>
public interface IUnitTypeProvider
{
    bool Exists(int unitTypeId);

    /// <summary>Returns the unit type, or throws when the id is unknown.</summary>
    UnitType Get(int unitTypeId);

    IReadOnlyCollection<UnitType> GetAll();
}
