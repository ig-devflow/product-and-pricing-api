using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

public sealed class TransferPortRepository : EfRepositoryBase<TransferPort, int>, ITransferPortRepository
{
    public TransferPortRepository(ProductsAndPricingDbContext db) : base(db) { }

    public override Task<TransferPort?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return Set
            .Include(x => x.Instructions)
            .Include(x => x.Terminals)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}