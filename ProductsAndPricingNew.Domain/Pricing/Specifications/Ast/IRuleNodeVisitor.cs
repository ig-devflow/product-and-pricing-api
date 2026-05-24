namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// Visitor over the specification AST. The pricing engine implements
/// <c>IRuleNodeVisitor&lt;Expression&gt;</c> to compile nodes into NRules conditions.
/// </summary>
public interface IRuleNodeVisitor<T>
{
    T VisitAnd(AndNode node);
    T VisitOr(OrNode node);
    T VisitNot(NotNode node);
    T VisitComparison(ComparisonNode node);
    T VisitIn(InNode node);
    T VisitHasValue(HasValueNode node);
    T VisitTrue(TrueNode node);
    T VisitFalse(FalseNode node);
}
