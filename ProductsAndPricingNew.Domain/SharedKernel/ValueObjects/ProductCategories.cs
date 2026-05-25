using ProductsAndPricingNew.Domain.Common.Primitives;

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

    public static ProductCategories Create(int accountCategoryId, int productCategoryId)
    {
        Guard.PositiveId(accountCategoryId, nameof(AccountCategoryId));
        Guard.PositiveId(productCategoryId, nameof(ProductCategoryId));

        return new ProductCategories(accountCategoryId, productCategoryId);
    }
}