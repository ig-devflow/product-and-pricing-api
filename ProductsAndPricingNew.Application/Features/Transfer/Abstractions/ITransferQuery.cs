using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Models;

namespace ProductsAndPricingNew.Application.Features.Transfer.Abstractions;

public interface ITransferQuery
{
    Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default);
    Task<TransferDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<PagedResult<TransferListItemDto>> GetListAsync(int divisionId, string? search, bool? isActive, PagingFilter paging, CancellationToken ct = default);
}
