using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Models;

namespace ProductsAndPricingNew.Application.Features.AddOn.Abstractions;

public interface IAddOnQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<AddOnDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<AddOnListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}
