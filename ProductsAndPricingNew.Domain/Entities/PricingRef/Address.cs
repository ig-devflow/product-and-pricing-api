namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Address : IEquatable<Address>
{
    private Address() { }

    public Address(int? countryId, params string?[] lines)
        : this(countryId, (IEnumerable<string?>)lines)
    {
    }

    public Address(int? countryId, IEnumerable<string?> lines)
    {
        var normalized = lines?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!.Trim())
            .Take(4)
            .ToArray()
            ?? Array.Empty<string>();

        CountryId = countryId;
        Line1 = normalized.Length > 0 ? normalized[0] : null;
        Line2 = normalized.Length > 1 ? normalized[1] : null;
        Line3 = normalized.Length > 2 ? normalized[2] : null;
        Line4 = normalized.Length > 3 ? normalized[3] : null;
    }

    public static Address Empty { get; } = new(null, Array.Empty<string?>());

    public string? Line1 { get; private set; }
    public string? Line2 { get; private set; }
    public string? Line3 { get; private set; }
    public string? Line4 { get; private set; }
    public int? CountryId { get; private set; }

    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(Line1) &&
        string.IsNullOrWhiteSpace(Line2) &&
        string.IsNullOrWhiteSpace(Line3) &&
        string.IsNullOrWhiteSpace(Line4) &&
        !CountryId.HasValue;

    public IEnumerable<string> GetLines()
    {
        if (!string.IsNullOrWhiteSpace(Line1)) yield return Line1!;
        if (!string.IsNullOrWhiteSpace(Line2)) yield return Line2!;
        if (!string.IsNullOrWhiteSpace(Line3)) yield return Line3!;
        if (!string.IsNullOrWhiteSpace(Line4)) yield return Line4!;
    }

    public bool Equals(Address? other)
    {
        if (other is null) 
            return false;

        return CountryId == other.CountryId
            && string.Equals(Line1, other.Line1, StringComparison.Ordinal)
            && string.Equals(Line2, other.Line2, StringComparison.Ordinal)
            && string.Equals(Line3, other.Line3, StringComparison.Ordinal)
            && string.Equals(Line4, other.Line4, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj) => obj is Address other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(CountryId, Line1, Line2, Line3, Line4);
}