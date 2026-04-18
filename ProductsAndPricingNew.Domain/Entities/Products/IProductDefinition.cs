namespace ProductsAndPricingNew.Domain.Entities.Products;

public interface IProductDefinition
{
    int DivisionId { get; }
    int UnitTypeId { get; }
    string Name { get; }
    bool IsActive { get; }
    DateOnly? OfferingsClosureDate { get; }
}