using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

public sealed class PackageRepository : EfRepositoryBase<Package, int>, IPackageRepository
{
    public PackageRepository(ProductsAndPricingDbContext db) : base(db) { }

    public override Task<Package?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return Set
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}