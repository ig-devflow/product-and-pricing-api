using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Abstractions;

public interface ICentreQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<CentreDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<CentreListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}