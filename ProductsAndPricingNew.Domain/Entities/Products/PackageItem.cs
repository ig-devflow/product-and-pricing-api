using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class PackageItem : Entity<int>, IEquatable<PackageItem>
{
    public ProductKind ProductKind { get; }
    public int ProductId { get; }
    public Percentage PriceBreakdown { get; private set; }

    public ProductRef Product => new(ProductKind, ProductId);

    private PackageItem() { }

    internal PackageItem(ProductRef product, Percentage priceBreakdown)
    {
        ProductKind = product.Kind;
        ProductId = product.Id;
        PriceBreakdown = priceBreakdown;
    }

    internal void ChangePercentage(Percentage percentage) => PriceBreakdown = percentage;

    public bool Equals(PackageItem? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;

        return ProductKind == other.ProductKind &&
               ProductId == other.ProductId &&
               PriceBreakdown.Equals(other.PriceBreakdown);
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is PackageItem other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)ProductKind, ProductId, PriceBreakdown);
    }

    public static bool operator ==(PackageItem left, PackageItem right) => left.Equals(right);

    public static bool operator !=(PackageItem left, PackageItem right) => !left.Equals(right);
}
