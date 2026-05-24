using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class AccommodationRoomRepository : EfRepositoryBase<AccommodationRoom, int>, IAccommodationRoomRepository
{
    public AccommodationRoomRepository(ProductsAndPricingDbContext db) : base(db) { }
}
