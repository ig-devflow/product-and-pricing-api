using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Abstractions;

public interface IDivisionQueries
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<DivisionDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<DivisionListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}