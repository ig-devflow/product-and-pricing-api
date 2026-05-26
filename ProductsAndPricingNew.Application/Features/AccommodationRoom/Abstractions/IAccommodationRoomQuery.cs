using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;

public interface IAccommodationRoomQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<AccommodationRoomDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<AccommodationRoomListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}