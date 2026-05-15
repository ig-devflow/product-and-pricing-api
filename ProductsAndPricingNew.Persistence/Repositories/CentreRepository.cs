using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class CentreRepository : EfRepositoryBase<Centre, int>, ICentreRepository
{
    public CentreRepository(ProductsAndPricingDbContext db) : base(db)
    {
    }

    public async Task<Centre?> GetByIdWithTextsAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Include(c => c.Contacts)
            .SingleOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
    }
}