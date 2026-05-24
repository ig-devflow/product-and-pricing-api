using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class PricingRuleRepository : IPricingRuleRepository
{
    private readonly ProductsAndPricingDbContext _db;

    public PricingRuleRepository(ProductsAndPricingDbContext db)
    {
        _db = db;
    }

    public async Task<PricingRule?> GetByIdAsync(int rulesetId, int ruleId, CancellationToken ct = default)
    {
        Ruleset? ruleset = await _db.Rulesets
            .Include("_rules")
            .FirstOrDefaultAsync(x => x.Id == rulesetId, ct);

        return ruleset?.Rules.FirstOrDefault(r => r.Id == ruleId);
    }
}
