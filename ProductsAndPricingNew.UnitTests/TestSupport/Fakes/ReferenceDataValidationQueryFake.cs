using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class ReferenceDataValidationQueryFake : IReferenceDataValidationQuery
{
    private readonly HashSet<int> _activeCountryIds = new();
    private readonly HashSet<int> _activeAudienceIds = new();
    private readonly Dictionary<ContentTemplateScope, HashSet<int>> _activeContentTemplateIds = new();

    public ReferenceDataValidationQueryFake WithActiveCountries(params int[] ids)
    {
        AddRange(_activeCountryIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveAudiences(params int[] ids)
    {
        AddRange(_activeAudienceIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveContentTemplates(ContentTemplateScope scope, params int[] ids)
    {
        if (!_activeContentTemplateIds.TryGetValue(scope, out HashSet<int>? activeIds))
        {
            activeIds = new HashSet<int>();
            _activeContentTemplateIds[scope] = activeIds;
        }

        AddRange(activeIds, ids);
        return this;
    }

    public Task<IReadOnlySet<int>> GetActiveCountryIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeCountryIds));

    public Task<IReadOnlySet<int>> GetActiveAudienceIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeAudienceIds));

    public Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(
        IReadOnlyCollection<int> ids,
        ContentTemplateScope scope,
        CancellationToken ct = default)
    {
        _activeContentTemplateIds.TryGetValue(scope, out HashSet<int>? activeIds);
        return Task.FromResult(Filter(ids, activeIds ?? new HashSet<int>()));
    }

    private static void AddRange(HashSet<int> target, IEnumerable<int> ids)
    {
        foreach (int id in ids)
            target.Add(id);
    }

    private static IReadOnlySet<int> Filter(IReadOnlyCollection<int> requestedIds, IReadOnlySet<int> activeIds)
        => requestedIds.Where(activeIds.Contains).ToHashSet();
}
