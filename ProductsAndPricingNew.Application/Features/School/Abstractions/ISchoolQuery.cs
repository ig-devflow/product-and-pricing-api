using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Abstractions;

public interface ISchoolQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<SchoolDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<SchoolListItemDto>> GetListAsync(string? search, bool? isActive, PagingFilter paging, int? centreId = null, CancellationToken ct = default);
}