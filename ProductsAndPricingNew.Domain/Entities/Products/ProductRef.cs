namespace ProductsAndPricingNew.Domain.Entities.Products;

public readonly struct ProductRef : IEquatable<ProductRef>, IComparable<ProductRef>, IComparable
{
    public ProductKind Kind { get; }
    public int Id { get; }
    
    public ProductRef(ProductKind kind, int id)
    {
        Kind = kind;
        Id = id;
    }
    
    public bool Equals(ProductRef other)
    {
        return Kind == other.Kind && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is ProductRef other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine((int)Kind, Id);
    }

    public int CompareTo(ProductRef other)
    {
        var kindCompare = Kind.CompareTo(other.Kind);
        if (kindCompare != 0)
            return kindCompare;

        return Id.CompareTo(other.Id);
    }

    int IComparable.CompareTo(object? obj)
    {
        if (obj == null)
            return 1;

        if (!(obj is ProductRef))
            throw new ArgumentException("Object must be of type ProductRef.", nameof(obj));

        return CompareTo((ProductRef)obj);
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", Kind, Id);
    }

    public static bool operator ==(ProductRef left, ProductRef right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ProductRef left, ProductRef right)
    {
        return !left.Equals(right);
    }

    public static bool operator <(ProductRef left, ProductRef right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator <=(ProductRef left, ProductRef right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >(ProductRef left, ProductRef right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator >=(ProductRef left, ProductRef right)
    {
        return left.CompareTo(right) >= 0;
    }
}