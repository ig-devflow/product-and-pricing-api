using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class DivisionQueryFake : IDivisionQuery
{
    private readonly Dictionary<string, int> _namesByDivisionId = new(StringComparer.OrdinalIgnoreCase);

    public int ExistsByNameCalls { get; private set; }
    public string? LastExistsByName { get; private set; }
    public int? LastExcludingId { get; private set; }

    public DivisionQueryFake WithExistingName(string name, int divisionId = 1)
    {
        _namesByDivisionId[name] = divisionId;
        return this;
    }

    public Task<bool> ExistsByNameAsync(string name, int? excludingId = null, CancellationToken ct = default)
    {
        ExistsByNameCalls++;
        LastExistsByName = name;
        LastExcludingId = excludingId;

        bool exists = _namesByDivisionId.TryGetValue(name, out int divisionId) &&
                      (!excludingId.HasValue || divisionId != excludingId.Value);

        return Task.FromResult(exists);
    }

    public Task<DivisionDetailsDto?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult<DivisionDetailsDto?>(null);

    public Task<PagedResult<DivisionListItemDto>> GetListAsync(
        string? search,
        bool? isActive,
        PagingFilter paging,
        CancellationToken ct = default)
    {
        PagedResult<DivisionListItemDto> result = new([], 0, paging.GetPage(), paging.GetPageSize());
        return Task.FromResult(result);
    }
}
