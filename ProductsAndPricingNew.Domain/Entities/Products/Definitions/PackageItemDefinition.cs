namespace ProductsAndPricingNew.Domain.Entities.Products.Definitions;

public sealed record PackageItemDefinition(
    ProductKind ProductKind,
    int ProductId,
    decimal PriceBreakdown
);