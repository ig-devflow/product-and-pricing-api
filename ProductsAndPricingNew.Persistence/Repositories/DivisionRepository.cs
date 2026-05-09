using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class DivisionRepository : EfRepositoryBase<Division, int>, IDivisionRepository
{
    public DivisionRepository(ProductsAndPricingDbContext db) : base(db)
    {
    }

    public async Task<Division?> GetByIdWithTextsAsync(int id, CancellationToken ct = default)
    {
        return await Db.Set<Division>()
            .Include(x => x.Texts)
            .SingleOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
    }
}