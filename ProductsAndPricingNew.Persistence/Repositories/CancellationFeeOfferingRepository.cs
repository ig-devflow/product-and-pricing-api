using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class CancellationFeeOfferingRepository : EfRepositoryBase<CancellationFeeOffering, int>, ICancellationFeeOfferingRepository
{
    public CancellationFeeOfferingRepository(ProductsAndPricingDbContext db) : base(db) { }
}
