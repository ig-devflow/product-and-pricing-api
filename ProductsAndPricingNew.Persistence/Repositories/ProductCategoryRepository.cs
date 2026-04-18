using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

public sealed class ProductCategoryRepository : EfRepositoryBase<ProductCategory, int>, IProductCategoryRepository
{
    public ProductCategoryRepository(ProductsAndPricingDbContext db) : base(db) { }
}
