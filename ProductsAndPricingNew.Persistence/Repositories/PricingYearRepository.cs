using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class PricingYearRepository : EfRepositoryBase<PricingYear, int>, IPricingYearRepository
{
    public PricingYearRepository(ProductsAndPricingDbContext db) : base(db) { }
}