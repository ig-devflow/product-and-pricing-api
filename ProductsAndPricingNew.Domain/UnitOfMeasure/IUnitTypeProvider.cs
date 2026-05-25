namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

public interface IUnitTypeProvider
{
    bool Exists(int unitTypeId);
    UnitType Get(int unitTypeId);
    IReadOnlyCollection<UnitType> GetAll();
}
