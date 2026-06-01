using ProductsAndPricingNew.Application.Features.ProductCategory.Models;

namespace ProductsAndPricingNew.Application.Features.ProductCategory.Abstractions;

public interface IProductCategoryQuery
{
    Task<bool> ExistsByNameAsync(string name, int divisionId, int? excludingId = null, CancellationToken ct = default);
    Task<ProductCategoryDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyCollection<ProductCategoryListItemDto>> GetListByDivisionAsync(int divisionId, CancellationToken ct = default);
}
