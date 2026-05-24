using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct ProductCategories : IEmptyValueObject
{
    public int AccountCategoryId { get; init; }
    public int ProductCategoryId { get; init; }

    public static readonly ProductCategories Unassigned = new(0, 0);
    public bool IsEmpty => AccountCategoryId <= 0 && ProductCategoryId <= 0;

    private ProductCategories(int accountCategoryId, int productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public static ProductCategories Create(int accountCategoryId, int productCategoryId)
    {
        if (accountCategoryId <= 0)
            throw new DomainException("AccountCategoryId must be greater than zero.");

        if (productCategoryId <= 0)
            throw new DomainException("ProductCategoryId must be greater than zero.");

        return new ProductCategories(accountCategoryId, productCategoryId);
    }

    public static ProductCategories Create(ProductCategoriesDefinition? definition) =>
        definition is null
            ? Unassigned
            : Create(definition.AccountCategoryId, definition.ProductCategoryId);
}