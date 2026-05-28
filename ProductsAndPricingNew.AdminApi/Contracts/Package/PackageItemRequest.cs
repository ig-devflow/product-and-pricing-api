using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.AdminApi.Contracts.Package;

public sealed record PackageItemRequest(
    ProductKind ProductKind,
    int ProductId,
    decimal PriceBreakdown
);
