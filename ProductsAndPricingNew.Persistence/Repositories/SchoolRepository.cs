using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class SchoolRepository : EfRepositoryBase<School, int>, ISchoolRepository
{
    public SchoolRepository(ProductsAndPricingDbContext db) : base(db)
    {
    }
}