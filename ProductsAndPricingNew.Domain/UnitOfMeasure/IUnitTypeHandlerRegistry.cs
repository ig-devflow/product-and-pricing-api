namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>Resolves the <see cref="IUnitTypeHandler"/> for a given <see cref="UnitCalculationKind"/>.</summary>
public interface IUnitTypeHandlerRegistry
{
    IUnitTypeHandler Resolve(UnitCalculationKind kind);
}
