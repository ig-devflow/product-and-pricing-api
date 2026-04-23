using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Persistence;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ProductsAndPricingDbContext _dbContext;

    public UnitOfWork(ProductsAndPricingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}