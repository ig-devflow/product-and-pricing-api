using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class DivisionRepository : EfRepositoryBase<Division, int>, IDivisionRepository
{
    public DivisionRepository(ProductsAndPricingDbContext db) : base(db)
    {
    }

    public async Task<Division?> GetByIdWithTextsAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Include(x => x.Texts)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }
}