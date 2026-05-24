using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class AccommodationRepository : EfRepositoryBase<Accommodation, int>, IAccommodationRepository
{
    public AccommodationRepository(ProductsAndPricingDbContext db) : base(db) { }
}