using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.Package.Models;

public record PackageItemDto(
    ProductKind ProductKind,
    int ProductId,
    decimal PriceBreakdown
);