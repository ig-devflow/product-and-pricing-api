using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ProductsAndPricingDbContext _dbContext;

    public UnitOfWork(ProductsAndPricingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}