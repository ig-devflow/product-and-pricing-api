using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class RulesetRepository : EfRepositoryBase<Ruleset, int>, IRulesetRepository
{
    public RulesetRepository(ProductsAndPricingDbContext db) : base(db) { }

    public override async Task<Ruleset?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Db.Rulesets
            .Include("_rules")
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
