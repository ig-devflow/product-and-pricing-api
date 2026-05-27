using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct FinanceCode : IEmptyValueObject
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

    public static class Rules
    {
        public const string DefaultValue = "ZZ";
        public const int MaxLength = 20;
    }
}