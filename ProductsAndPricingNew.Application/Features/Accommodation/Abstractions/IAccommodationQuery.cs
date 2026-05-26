using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;

public interface IAccommodationQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<AccommodationDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<AccommodationListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}