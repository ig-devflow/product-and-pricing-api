using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Abstractions;

public interface IPackageQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<PackageDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<PackageListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}
