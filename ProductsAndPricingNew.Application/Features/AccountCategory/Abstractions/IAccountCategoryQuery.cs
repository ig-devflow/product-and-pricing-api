using ProductsAndPricingNew.Application.Features.AccountCategory.Models;

namespace ProductsAndPricingNew.Application.Features.AccountCategory.Abstractions;

public interface IAccountCategoryQuery
{
    Task<bool> ExistsByNameAsync(string name, int divisionId, int? excludingId = null, CancellationToken ct = default);
    Task<AccountCategoryDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyCollection<AccountCategoryListItemDto>> GetListByDivisionAsync(int divisionId, CancellationToken ct = default);
}
