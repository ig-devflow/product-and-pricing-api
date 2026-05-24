using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Discounts;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class DiscountRepository : EfRepositoryBase<Discount, int>, IDiscountRepository
{
    public DiscountRepository(ProductsAndPricingDbContext db) : base(db) { }
}
