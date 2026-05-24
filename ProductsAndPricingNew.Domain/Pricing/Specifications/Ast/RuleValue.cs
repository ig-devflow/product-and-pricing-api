namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// A typed literal inside the specification AST — the modern replacement for the legacy
/// <c>IConstantValueType</c>. <see cref="Value"/> is always non-null and boxed to the CLR
/// type implied by <see cref="Type"/>: String/Enum → string, Int → long, Decimal → decimal,
/// Date → DateOnly, Bool → bool.
/// </summary>
public sealed record RuleValue
{
    public RuleValueType Type { get; }
    public object Value { get; }

    private RuleValue(RuleValueType type, object value)
    {
        Type = type;
        Value = value;
    }

    public static RuleValue String(string value) =>
        new(RuleValueType.String, value ?? throw new ArgumentNullException(nameof(value)));

    public static RuleValue Int(long value) => new(RuleValueType.Int, value);

    public static RuleValue Decimal(decimal value) => new(RuleValueType.Decimal, value);

    public static RuleValue Date(DateOnly value) => new(RuleValueType.Date, value);

    public static RuleValue Bool(bool value) => new(RuleValueType.Bool, value);

    public static RuleValue Enum(string memberName) =>
        new(RuleValueType.Enum, memberName ?? throw new ArgumentNullException(nameof(memberName)));

    public static RuleValue Enum<TEnum>(TEnum value) where TEnum : struct, System.Enum =>
        new(RuleValueType.Enum, value.ToString()!);

    /// <summary>Reconstructs a value from already-typed parts (used by JSON deserialization).</summary>
    public static RuleValue FromParts(RuleValueType type, object value) =>
        new(type, value ?? throw new ArgumentNullException(nameof(value)));
}
