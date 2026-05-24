using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

namespace ProductsAndPricingNew.Pricing.Engine;

/// <summary>
/// Output fact produced when a rule matches: identifies the ruleset and the rule (by name),
/// and carries the <see cref="RuleFact"/> that satisfied it.
/// </summary>
public sealed record RuleMatch(int RulesetId, string RuleName, RuleFact Fact);
