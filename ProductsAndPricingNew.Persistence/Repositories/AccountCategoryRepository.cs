using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

internal sealed class AccountCategoryRepository : EfRepositoryBase<AccountCategory, int>, IAccountCategoryRepository
{
    public AccountCategoryRepository(ProductsAndPricingDbContext db) : base(db) { }
}