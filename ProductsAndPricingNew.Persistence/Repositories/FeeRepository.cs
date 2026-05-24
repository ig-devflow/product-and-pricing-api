using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class FeeRepository : EfRepositoryBase<Fee, int>, IFeeRepository
{
    public FeeRepository(ProductsAndPricingDbContext db) : base(db) { }
}
