using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

public sealed class AddOnRepository : EfRepositoryBase<AddOn, int>, IAddOnRepository
{
    public AddOnRepository(ProductsAndPricingDbContext db) : base(db) { }
}