using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct WebsiteUrl : IEquatable<WebsiteUrl>, IEmptyValueObject
{
    public string? Value { get; }

    private WebsiteUrl(string? value)
    {
        Value = value;
    }

    public bool IsEmpty => Value is null;
    public static WebsiteUrl Empty { get; } = new(null);

    public static WebsiteUrl Create(string? value)
    {
        string? normalized = value.AsOptionalDomainText(nameof(WebsiteUrl), Rules.MaxLength);

        if (normalized is null)
            return Empty;

        EnsureValid(normalized);

        return new WebsiteUrl(normalized);
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string normalized = value.AsOptionalText()!;

        return normalized.Length <= Rules.MaxLength && IsValidUrl(normalized);
    }

    private static void EnsureValid(string value)
    {
        if (!IsValidUrl(value))
            throw new DomainException("WebsiteUrl must be a valid HTTP or HTTPS URL.");
    }

    private static bool IsValidUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
            return false;

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
            return false;

        return !string.IsNullOrWhiteSpace(uri.Host);
    }

    public bool Equals(WebsiteUrl other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) =>
        obj is WebsiteUrl other && Equals(other);

    public override int GetHashCode() =>
        Value is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value ?? string.Empty;

    public static bool operator ==(WebsiteUrl left, WebsiteUrl right) => left.Equals(right);
    public static bool operator !=(WebsiteUrl left, WebsiteUrl right) => !left.Equals(right);

    public static class Rules
    {
        public const int MaxLength = 255;
    }
}