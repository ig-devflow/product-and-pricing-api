using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct FinanceCode : IEquatable<FinanceCode>, IEmptyValueObject
{
    public string Value { get; }

    private FinanceCode(string value)
    {
        Value = value;
    }

    public bool IsEmpty => string.Equals(Value, Rules.DefaultValue, StringComparison.OrdinalIgnoreCase);
    public static FinanceCode Empty { get; } = new(Rules.DefaultValue);

    public static FinanceCode Create(string? value)
    {
        string normalized = value.AsOptionalDomainText(nameof(FinanceCode), Rules.MaxLength) ?? Rules.DefaultValue;
        return new FinanceCode(normalized.ToUpperInvariant());
    }

    public static bool IsValid(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return true;

        string normalized = value.AsOptionalText()!;

        return normalized.Length <= Rules.MaxLength;
    }

    public bool Equals(FinanceCode other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) =>
        obj is FinanceCode other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public override string ToString() => Value;

    public static bool operator ==(FinanceCode left, FinanceCode right) => left.Equals(right);
    public static bool operator !=(FinanceCode left, FinanceCode right) => !left.Equals(right);

    public static class Rules
    {
        public const string DefaultValue = "ZZ";
        public const int MaxLength = 20;
    }
}