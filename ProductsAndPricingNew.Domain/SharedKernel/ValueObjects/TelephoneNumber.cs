using System.Text.RegularExpressions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct TelephoneNumber : IEquatable<TelephoneNumber>, IEmptyValueObject
{
    private static readonly Regex PhoneRegex = new(@"^[\d\s\-\(\)\+]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1));

    public string? Value { get; }

    private TelephoneNumber(string? value)
    {
        Value = value;
    }

    public bool IsEmpty => Value is null;
    public static TelephoneNumber Empty { get; } = new(null);

    public static TelephoneNumber Create(string? value)
    {
        string? normalized = value.AsOptionalDomainText(nameof(TelephoneNumber), Rules.MaxLength);

        if (normalized is null)
            return Empty;

        EnsureValid(normalized);

        return new TelephoneNumber(normalized);
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string normalized = value.AsOptionalText()!;

        return normalized.Length <= Rules.MaxLength && IsValidNumber(normalized);
    }

    private static void EnsureValid(string value)
    {
        if (!IsValidNumber(value))
            throw new DomainException($"Telephone number must include area/country code and contain {Rules.MinDigits}-{Rules.MaxDigits} digits.");
    }

    private static bool IsValidNumber(string value)
    {
        if (!PhoneRegex.IsMatch(value))
            return false;

        int digitsCount = value.Count(char.IsDigit);

        return digitsCount is >= Rules.MinDigits and <= Rules.MaxDigits;
    }

    public bool Equals(TelephoneNumber other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) =>
        obj is TelephoneNumber other && Equals(other);

    public override int GetHashCode() =>
        Value is null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value ?? string.Empty;

    public static bool operator ==(TelephoneNumber left, TelephoneNumber right) => left.Equals(right);
    public static bool operator !=(TelephoneNumber left, TelephoneNumber right) => !left.Equals(right);

    public static class Rules
    {
        public const int MaxLength = 50;
        public const int MinDigits = 8;
        public const int MaxDigits = 20;
    }
}