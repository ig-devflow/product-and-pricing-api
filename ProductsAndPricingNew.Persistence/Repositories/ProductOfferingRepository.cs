using ProductsAndPricingNew.Domain.Entities.Offerings;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class ProductOfferingRepository : EfRepositoryBase<ProductOffering, int>, IProductOfferingRepository
{
    public ProductOfferingRepository(ProductsAndPricingDbContext db) : base(db) { }
}