using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Repositories;

public interface IRepository<TAggregate, TId> where TAggregate : AggregateRoot<TId>
{
    Task<TAggregate?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<bool> ExistsAsync(TId id, CancellationToken ct = default);
    Task AddAsync(TAggregate aggregate, CancellationToken ct = default);
    void Remove(TAggregate aggregate);
}
