using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.Domain.Abstractions;

public interface IDivisionRepository : IRepository<Division, int>
{
    Task<Division?> GetByIdWithTextsAsync(int id, CancellationToken ct = default);
}
