using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct FinanceCode : IEquatable<FinanceCode>, IEmptyValueObject
{
    private readonly string? _value;

    public string Value => _value ?? Rules.DefaultValue;

    private FinanceCode(string value)
    {
        _value = value;
    }

    public bool IsEmpty => string.Equals(Value, Rules.DefaultValue, StringComparison.OrdinalIgnoreCase);

    public static FinanceCode Empty { get; } = new(Rules.DefaultValue);

    public static FinanceCode Create(string? value, string fieldName = nameof(FinanceCode), int maxLength = Rules.MaxLength)
    {
        string normalized = value.AsOptionalDomainText(fieldName, maxLength) ?? Rules.DefaultValue;
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