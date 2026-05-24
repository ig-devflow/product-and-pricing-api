using System.Globalization;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// Type-aware evaluation of a fact value against a <see cref="RuleValue"/> literal. Shared
/// by the AST interpreter and the pricing engine's expression compiler so an interpreted
/// rule and a compiled rule always agree.
/// </summary>
public static class RuleValueMatcher
{
    /// <summary>Evaluates a single comparison leaf.</summary>
    public static bool Satisfies(object? factValue, ComparisonOperator op, RuleValue value) =>
        op switch
        {
            ComparisonOperator.Eq => IsEqual(factValue, value),
            ComparisonOperator.NotEq => factValue is not null && !IsEqual(factValue, value),
            ComparisonOperator.Lt => Compare(factValue, value) is int lt && lt < 0,
            ComparisonOperator.Lte => Compare(factValue, value) is int lte && lte <= 0,
            ComparisonOperator.Gt => Compare(factValue, value) is int gt && gt > 0,
            ComparisonOperator.Gte => Compare(factValue, value) is int gte && gte >= 0,
            _ => false
        };

    /// <summary>Evaluates an "in set" leaf.</summary>
    public static bool IsIn(object? factValue, IReadOnlyList<RuleValue> values)
    {
        if (factValue is null)
            return false;

        foreach (RuleValue value in values)
        {
            if (IsEqual(factValue, value))
                return true;
        }

        return false;
    }

    private static bool IsEqual(object? factValue, RuleValue ruleValue)
    {
        if (factValue is null)
            return false;

        return ruleValue.Type switch
        {
            RuleValueType.String => string.Equals(AsString(factValue), (string)ruleValue.Value, StringComparison.Ordinal),
            RuleValueType.Enum => string.Equals(AsString(factValue), (string)ruleValue.Value, StringComparison.Ordinal),
            RuleValueType.Bool => factValue is bool b && b == (bool)ruleValue.Value,
            RuleValueType.Date => TryAsDate(factValue, out DateOnly d) && d == (DateOnly)ruleValue.Value,
            RuleValueType.Int => TryAsDecimal(factValue, out decimal i) && i == (long)ruleValue.Value,
            RuleValueType.Decimal => TryAsDecimal(factValue, out decimal m) && m == (decimal)ruleValue.Value,
            _ => false
        };
    }

    private static int? Compare(object? factValue, RuleValue ruleValue)
    {
        if (factValue is null)
            return null;

        switch (ruleValue.Type)
        {
            case RuleValueType.Int:
                return TryAsDecimal(factValue, out decimal i) ? i.CompareTo((long)ruleValue.Value) : null;
            case RuleValueType.Decimal:
                return TryAsDecimal(factValue, out decimal m) ? m.CompareTo((decimal)ruleValue.Value) : null;
            case RuleValueType.Date:
                return TryAsDate(factValue, out DateOnly d) ? d.CompareTo((DateOnly)ruleValue.Value) : null;
            case RuleValueType.String:
                return string.CompareOrdinal(AsString(factValue), (string)ruleValue.Value);
            default:
                return null;
        }
    }

    private static string AsString(object value) =>
        value as string ?? Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;

    private static bool TryAsDate(object value, out DateOnly date)
    {
        switch (value)
        {
            case DateOnly d:
                date = d;
                return true;
            case DateTime dt:
                date = DateOnly.FromDateTime(dt);
                return true;
            default:
                date = default;
                return false;
        }
    }

    private static bool TryAsDecimal(object value, out decimal number)
    {
        switch (value)
        {
            case decimal d: number = d; return true;
            case int i: number = i; return true;
            case long l: number = l; return true;
            case short s: number = s; return true;
            case byte b: number = b; return true;
            case double db: number = (decimal)db; return true;
            case float f: number = (decimal)f; return true;
            default: number = 0m; return false;
        }
    }
}
