using System.Text.Json.Serialization;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// A node of the specification AST — the modern, serializable replacement for the legacy
/// <c>ICriteria</c> tree. Stored as JSON in <c>PricingRule.ScriptJson</c> and compiled into
/// NRules conditions by the pricing engine.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "op")]
[JsonDerivedType(typeof(AndNode), "and")]
[JsonDerivedType(typeof(OrNode), "or")]
[JsonDerivedType(typeof(NotNode), "not")]
[JsonDerivedType(typeof(ComparisonNode), "cmp")]
[JsonDerivedType(typeof(InNode), "in")]
[JsonDerivedType(typeof(HasValueNode), "has")]
[JsonDerivedType(typeof(TrueNode), "true")]
[JsonDerivedType(typeof(FalseNode), "false")]
public abstract record RuleNode
{
    /// <summary>
    /// In-memory interpreter — used for unit tests and the UI "preview rule" feature; the
    /// production path compiles the AST into NRules. A comparison/in leaf evaluates to
    /// <c>false</c> when its field is absent or null — use <see cref="HasValueNode"/> and
    /// <see cref="NotNode"/> to express presence logic.
    /// </summary>
    public abstract bool IsSatisfiedBy(RuleFact fact);

    public abstract T Accept<T>(IRuleNodeVisitor<T> visitor);
}

public sealed record AndNode(IReadOnlyList<RuleNode> Nodes) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact)
    {
        foreach (RuleNode node in Nodes)
        {
            if (!node.IsSatisfiedBy(fact))
                return false;
        }

        return true;
    }

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitAnd(this);
}

public sealed record OrNode(IReadOnlyList<RuleNode> Nodes) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact)
    {
        foreach (RuleNode node in Nodes)
        {
            if (node.IsSatisfiedBy(fact))
                return true;
        }

        return false;
    }

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitOr(this);
}

public sealed record NotNode(RuleNode Node) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) => !Node.IsSatisfiedBy(fact);

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitNot(this);
}

public sealed record ComparisonNode(string Field, ComparisonOperator Op, RuleValue Value) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) =>
        RuleValueMatcher.Satisfies(fact.Get(Field), Op, Value);

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitComparison(this);
}

public sealed record InNode(string Field, IReadOnlyList<RuleValue> Values) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) =>
        RuleValueMatcher.IsIn(fact.Get(Field), Values);

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitIn(this);
}

public sealed record HasValueNode(string Field) : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) => fact.Has(Field);

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitHasValue(this);
}

public sealed record TrueNode : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) => true;

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitTrue(this);
}

public sealed record FalseNode : RuleNode
{
    public override bool IsSatisfiedBy(RuleFact fact) => false;

    public override T Accept<T>(IRuleNodeVisitor<T> visitor) => visitor.VisitFalse(this);
}
