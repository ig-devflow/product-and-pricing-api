using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct HexColor : IEquatable<HexColor>, IEmptyValueObject
{
    public string? Value { get; }

    private HexColor(string? value)
    {
        Value = value;
    }

    public bool IsEmpty => Value is null;

    public static HexColor Empty { get; } = new(null);

    public static HexColor Create(string? value)
    {
        string? normalized = value.AsOptionalDomainText(nameof(HexColor), Rules.MaxLengthWithHash);

        if (normalized is null)
            return Empty;

        return new HexColor(Normalize(normalized));
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string normalized = value.AsOptionalText()!;

        if (normalized.Length > Rules.MaxLengthWithHash)
            return false;

        string color = normalized.StartsWith('#') ? normalized[1..] : normalized;

        if (color.Length != Rules.ShortLength && color.Length != Rules.FullLength)
            return false;

        return color.All(Uri.IsHexDigit);
    }

    private static string Normalize(string value)
    {
        string color = value.StartsWith('#') ? value[1..] : value;

        if (color.Length != Rules.ShortLength && color.Length != Rules.FullLength)
            throw new DomainException("HexColor must be a 3 or 6 digit hex color.");

        foreach (char symbol in color)
        {
            if (!Uri.IsHexDigit(symbol))
                throw new DomainException("HexColor must contain only hex digits.");
        }

        color = color.ToUpperInvariant();

        if (color.Length == Rules.ShortLength)
            color = $"{color[0]}{color[0]}{color[1]}{color[1]}{color[2]}{color[2]}";

        return $"#{color}";
    }

    public bool Equals(HexColor other) =>
        string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) =>
        obj is HexColor other && Equals(other);

    public override int GetHashCode() =>
        Value is null ? 0 : StringComparer.Ordinal.GetHashCode(Value);

    public override string ToString() => Value ?? string.Empty;

    public static bool operator ==(HexColor left, HexColor right) => left.Equals(right);
    public static bool operator !=(HexColor left, HexColor right) => !left.Equals(right);

    public static class Rules
    {
        public const int ShortLength = 3;
        public const int FullLength = 6;
        public const int MaxLengthWithHash = 7;
    }
}