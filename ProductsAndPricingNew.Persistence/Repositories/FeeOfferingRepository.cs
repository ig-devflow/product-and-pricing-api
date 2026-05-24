using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class FeeOfferingRepository : EfRepositoryBase<FeeOffering, int>, IFeeOfferingRepository
{
    public FeeOfferingRepository(ProductsAndPricingDbContext db) : base(db) { }
}