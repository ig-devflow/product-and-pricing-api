using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public interface IProductDefinition
{
    int Id { get; }
    int DivisionId { get; }
    int UnitTypeId { get; }
    string Name { get; }
    bool IsActive { get; }
    FinanceCodes FinanceCodes { get; }
    OfferingsClosurePolicy ClosurePolicy { get; }
}