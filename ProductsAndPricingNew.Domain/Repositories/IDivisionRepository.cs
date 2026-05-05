using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Domain.Repositories;

public interface IDivisionRepository : IRepository<Division, int>
{
    Task<Division?> GetByIdWithTextsAsync(int id, CancellationToken ct = default);
}