using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Tax;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class TaxRegimenRepository : EfRepositoryBase<TaxRegimen, int>, ITaxRegimenRepository
{
    public TaxRegimenRepository(ProductsAndPricingDbContext db) : base(db) { }

    public override async Task<TaxRegimen?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await Set
            .Include("_bands")
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
