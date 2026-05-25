using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct ProductCategories : IEmptyValueObject
{
    public int AccountCategoryId { get; }
    public int ProductCategoryId { get; }

    public static readonly ProductCategories Unassigned = new(0, 0);
    public bool IsEmpty => AccountCategoryId <= 0 && ProductCategoryId <= 0;

    private ProductCategories(int accountCategoryId, int productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public static ProductCategories Create(ProductCategoriesDefinition? definition)
    {
        if (definition is null)
            return Unassigned;

        Guard.PositiveId(definition.AccountCategoryId, nameof(AccountCategoryId));
        Guard.PositiveId(definition.ProductCategoryId, nameof(ProductCategoryId));

        return new ProductCategories(definition.AccountCategoryId, definition.ProductCategoryId);
    }
}