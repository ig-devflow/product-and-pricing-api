using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class CancellationFeeRepository : EfRepositoryBase<CancellationFee, int>, ICancellationFeeRepository
{
    public CancellationFeeRepository(ProductsAndPricingDbContext db) : base(db) { }
}
