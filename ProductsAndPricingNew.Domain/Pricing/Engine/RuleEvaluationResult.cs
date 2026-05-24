namespace ProductsAndPricingNew.Domain.Pricing.Engine;

public readonly record struct RuleEvaluationResult(bool IsSatisfied, string? Reason = null);
