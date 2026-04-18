namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class ProductCategory : CategoryBase
{
    private ProductCategory() { }

    public ProductCategory(int id, int divisionId, string name)
        : base(id, divisionId, name)
    {
    }
}