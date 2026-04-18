using Microsoft.EntityFrameworkCore;
using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Persistence.Repositories;

public abstract class EfRepositoryBase<TAggregate, TId> : IRepository<TAggregate, TId>
    where TAggregate : AggregateRoot<TId>
{
    protected readonly ProductsAndPricingDbContext Db;
    protected DbSet<TAggregate> Set => Db.Set<TAggregate>();

    protected EfRepositoryBase(ProductsAndPricingDbContext db)
    {
        Db = db;
    }

    public virtual async Task<TAggregate?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        return await Set.FindAsync(new object?[] { id }, ct);
    }

    public virtual async Task<bool> ExistsAsync(TId id, CancellationToken ct = default)
    {
        return await GetByIdAsync(id, ct) is not null;
    }

    public virtual async Task AddAsync(TAggregate aggregate, CancellationToken ct = default)
    {
        await Set.AddAsync(aggregate, ct);
    }

    public virtual void Remove(TAggregate aggregate)
    {
        Set.Remove(aggregate);
    }
}