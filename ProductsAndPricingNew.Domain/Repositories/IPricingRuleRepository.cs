using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Domain.Repositories;

public interface IPricingRuleRepository
{
    Task<PricingRule?> GetByIdAsync(int rulesetId, int ruleId, CancellationToken ct = default);
}
