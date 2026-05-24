using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class TransferRepository : EfRepositoryBase<Transfer, int>, ITransferRepository
{
    public TransferRepository(ProductsAndPricingDbContext db) : base(db) { }
}
