using System.Linq.Expressions;
using System.Reflection;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

namespace ProductsAndPricingNew.Pricing.Compilation;

/// <summary>
/// Compiles a specification AST (<see cref="RuleNode"/>) into an expression tree over a
/// <see cref="RuleFact"/> — the form NRules consumes as a rule condition. Leaf nodes call
/// <see cref="RuleValueMatcher"/>, the same evaluator used by the AST interpreter, so a
/// compiled rule and an interpreted rule always agree.
/// </summary>
public sealed class RuleNodeExpressionCompiler : IRuleNodeVisitor<Expression>
{
    private static readonly MethodInfo GetMethod =
        typeof(RuleFact).GetMethod(nameof(RuleFact.Get))!;

    private static readonly MethodInfo HasMethod =
        typeof(RuleFact).GetMethod(nameof(RuleFact.Has))!;

    private static readonly MethodInfo SatisfiesMethod =
        typeof(RuleValueMatcher).GetMethod(nameof(RuleValueMatcher.Satisfies))!;

    private static readonly MethodInfo IsInMethod =
        typeof(RuleValueMatcher).GetMethod(nameof(RuleValueMatcher.IsIn))!;

    private readonly ParameterExpression _fact;

    private RuleNodeExpressionCompiler(ParameterExpression fact) => _fact = fact;

    /// <summary>Compiles the AST into the lambda <c>fact =&gt; &lt;predicate&gt;</c>.</summary>
    public static Expression<Func<RuleFact, bool>> Compile(RuleNode node)
    {
        ParameterExpression fact = Expression.Parameter(typeof(RuleFact), "fact");
        Expression body = node.Accept(new RuleNodeExpressionCompiler(fact));
        return Expression.Lambda<Func<RuleFact, bool>>(body, fact);
    }

    public Expression VisitAnd(AndNode node)
    {
        Expression result = Expression.Constant(true);
        foreach (RuleNode child in node.Nodes)
            result = Expression.AndAlso(result, child.Accept(this));

        return result;
    }

    public Expression VisitOr(OrNode node)
    {
        Expression result = Expression.Constant(false);
        foreach (RuleNode child in node.Nodes)
            result = Expression.OrElse(result, child.Accept(this));

        return result;
    }

    public Expression VisitNot(NotNode node) => Expression.Not(node.Node.Accept(this));

    public Expression VisitComparison(ComparisonNode node) =>
        Expression.Call(
            SatisfiesMethod,
            FieldValue(node.Field),
            Expression.Constant(node.Op),
            Expression.Constant(node.Value));

    public Expression VisitIn(InNode node) =>
        Expression.Call(
            IsInMethod,
            FieldValue(node.Field),
            Expression.Constant(node.Values, typeof(IReadOnlyList<RuleValue>)));

    public Expression VisitHasValue(HasValueNode node) =>
        Expression.Call(_fact, HasMethod, Expression.Constant(node.Field, typeof(string)));

    public Expression VisitTrue(TrueNode node) => Expression.Constant(true);

    public Expression VisitFalse(FalseNode node) => Expression.Constant(false);

    private MethodCallExpression FieldValue(string field) =>
        Expression.Call(_fact, GetMethod, Expression.Constant(field, typeof(string)));
}
