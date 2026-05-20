namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class ProductCategory : CategoryBase
{
    private ProductCategory() { }

    private ProductCategory(int divisionId, string name, bool isActive)
        : base(divisionId, name, isActive)
    {
    }

    public static ProductCategory Create(int divisionId, string name, bool isActive)
    {
        return new ProductCategory(divisionId, name, isActive);
    }
}