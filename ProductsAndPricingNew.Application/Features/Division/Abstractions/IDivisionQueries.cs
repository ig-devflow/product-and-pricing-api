using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Abstractions;

public interface IDivisionQueries
{
    Task<DivisionDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<PagedResult<DivisionListItemDto>> GetListAsync(
        string? search,
        bool? isActive,
        PagingFilter paging,
        CancellationToken ct = default);
}