using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Models;

namespace ProductsAndPricingNew.Application.Features.Course.Abstractions;

public interface ICourseQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<CourseDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<CourseListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}
