namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>Fluent factory for composing specification AST trees in code (and in tests).</summary>
public static class RuleSpec
{
    public static RuleNode True { get; } = new TrueNode();
    public static RuleNode False { get; } = new FalseNode();

    public static RuleNode And(params RuleNode[] nodes) => new AndNode(nodes);
    public static RuleNode And(IEnumerable<RuleNode> nodes) => new AndNode(nodes.ToList());

    public static RuleNode Or(params RuleNode[] nodes) => new OrNode(nodes);
    public static RuleNode Or(IEnumerable<RuleNode> nodes) => new OrNode(nodes.ToList());

    public static RuleNode Not(RuleNode node) => new NotNode(node);

    public static RuleFieldRef Field(string field) => new(field);
}

/// <summary>A field reference produced by <see cref="RuleSpec.Field"/>; builds leaf nodes.</summary>
public readonly struct RuleFieldRef
{
    private readonly string _field;

    internal RuleFieldRef(string field) =>
        _field = string.IsNullOrWhiteSpace(field)
            ? throw new ArgumentException("Field key is required.", nameof(field))
            : field;

    private RuleNode Cmp(ComparisonOperator op, RuleValue value) => new ComparisonNode(_field, op, value);

    public RuleNode Eq(RuleValue value) => Cmp(ComparisonOperator.Eq, value);
    public RuleNode Eq(long value) => Eq(RuleValue.Int(value));
    public RuleNode Eq(decimal value) => Eq(RuleValue.Decimal(value));
    public RuleNode Eq(string value) => Eq(RuleValue.String(value));
    public RuleNode Eq(DateOnly value) => Eq(RuleValue.Date(value));
    public RuleNode Eq(bool value) => Eq(RuleValue.Bool(value));
    public RuleNode Eq<TEnum>(TEnum value) where TEnum : struct, Enum => Eq(RuleValue.Enum(value));

    public RuleNode NotEq(RuleValue value) => Cmp(ComparisonOperator.NotEq, value);
    public RuleNode NotEq(long value) => NotEq(RuleValue.Int(value));
    public RuleNode NotEq(string value) => NotEq(RuleValue.String(value));
    public RuleNode NotEq(bool value) => NotEq(RuleValue.Bool(value));
    public RuleNode NotEq<TEnum>(TEnum value) where TEnum : struct, Enum => NotEq(RuleValue.Enum(value));

    public RuleNode Lt(RuleValue value) => Cmp(ComparisonOperator.Lt, value);
    public RuleNode Lt(long value) => Lt(RuleValue.Int(value));
    public RuleNode Lt(decimal value) => Lt(RuleValue.Decimal(value));
    public RuleNode Lt(DateOnly value) => Lt(RuleValue.Date(value));

    public RuleNode Lte(RuleValue value) => Cmp(ComparisonOperator.Lte, value);
    public RuleNode Lte(long value) => Lte(RuleValue.Int(value));
    public RuleNode Lte(decimal value) => Lte(RuleValue.Decimal(value));
    public RuleNode Lte(DateOnly value) => Lte(RuleValue.Date(value));

    public RuleNode Gt(RuleValue value) => Cmp(ComparisonOperator.Gt, value);
    public RuleNode Gt(long value) => Gt(RuleValue.Int(value));
    public RuleNode Gt(decimal value) => Gt(RuleValue.Decimal(value));
    public RuleNode Gt(DateOnly value) => Gt(RuleValue.Date(value));

    public RuleNode Gte(RuleValue value) => Cmp(ComparisonOperator.Gte, value);
    public RuleNode Gte(long value) => Gte(RuleValue.Int(value));
    public RuleNode Gte(decimal value) => Gte(RuleValue.Decimal(value));
    public RuleNode Gte(DateOnly value) => Gte(RuleValue.Date(value));

    public RuleNode In(params RuleValue[] values) => new InNode(_field, values);
    public RuleNode In(params long[] values) => new InNode(_field, values.Select(v => RuleValue.Int(v)).ToList());
    public RuleNode In(params string[] values) => new InNode(_field, values.Select(v => RuleValue.String(v)).ToList());
    public RuleNode In<TEnum>(params TEnum[] values) where TEnum : struct, Enum => new InNode(_field, values.Select(v => RuleValue.Enum(v)).ToList());

    public RuleNode HasValue() => new HasValueNode(_field);
}
