using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Pricing.Engine;

public interface IRuleEngine
{
    Task<RuleEvaluationResult> EvaluateAsync(
        RulesetRef ruleset,
        RuleEvaluationContext context,
        CancellationToken ct = default);
}
