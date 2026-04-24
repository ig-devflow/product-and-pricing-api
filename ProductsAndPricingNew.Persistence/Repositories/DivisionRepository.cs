using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class DivisionRepository : EfRepositoryBase<Division, int>, IDivisionRepository
{
    public DivisionRepository(ProductsAndPricingDbContext db) : base(db) { }
}