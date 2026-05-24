namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

/// <summary>
/// The full set of operations a field can offer in the rule-builder UI — a superset of
/// <see cref="Ast.ComparisonOperator"/> that also covers In / NotIn / HasValue.
/// </summary>
public enum RuleOperator
{
    Eq = 1,
    NotEq = 2,
    Lt = 3,
    Lte = 4,
    Gt = 5,
    Gte = 6,
    In = 7,
    NotIn = 8,
    HasValue = 9
}
