using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Domain.Repositories;

public interface ICentreRepository : IRepository<Centre, int>
{
    Task<Centre?> GetByIdWithTextsAsync(int id, CancellationToken ct = default);
}