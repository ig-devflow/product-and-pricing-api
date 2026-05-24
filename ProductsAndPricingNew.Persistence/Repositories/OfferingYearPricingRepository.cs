using ProductsAndPricingNew.Domain.Entities.Pricing;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class OfferingYearPricingRepository : EfRepositoryBase<OfferingYearPricing, int>, IOfferingYearPricingRepository
{
    public OfferingYearPricingRepository(ProductsAndPricingDbContext db) : base(db) { }
}