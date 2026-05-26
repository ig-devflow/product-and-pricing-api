using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class ReferenceDataValidationQueryFake : IReferenceDataValidationQuery
{
    private readonly HashSet<int> _activeCountryIds = new();
    private readonly HashSet<int> _activeAudienceIds = new();
    private readonly HashSet<int> _activeCurrencyIds = new();
    private readonly HashSet<int> _activeUnitTypeIds = new();
    private readonly HashSet<int> _activePrintFormatIds = new();
    private readonly HashSet<int> _activeContactTypeIds = new();

    private readonly HashSet<int> _activeAccommodationRoomTypeIds = new();
    private readonly HashSet<int> _activeAccommodationBoardTypeIds = new();
    private readonly HashSet<int> _activeAccommodationBathroomTypeIds = new();
    private readonly HashSet<int> _activeAccommodationRoomGradeIds = new();
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

    public ReferenceDataValidationQueryFake WithActiveCurrencies(params int[] ids)
    {
        AddRange(_activeCurrencyIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActivePrintFormats(params int[] ids)
    {
        AddRange(_activePrintFormatIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveContactTypes(params int[] ids)
    {
        AddRange(_activeContactTypeIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveUnitTypes(params int[] ids)
    {
        AddRange(_activeUnitTypeIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveAccommodationRoomTypes(params int[] ids)
    {
        AddRange(_activeAccommodationRoomTypeIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveAccommodationBoardTypes(params int[] ids)
    {
        AddRange(_activeAccommodationBoardTypeIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveAccommodationBathroomTypes(params int[] ids)
    {
        AddRange(_activeAccommodationBathroomTypeIds, ids);
        return this;
    }

    public ReferenceDataValidationQueryFake WithActiveAccommodationRoomGrades(params int[] ids)
    {
        AddRange(_activeAccommodationRoomGradeIds, ids);
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

    public Task<IReadOnlySet<int>> GetActiveCurrencyIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeCurrencyIds));

    public Task<IReadOnlySet<int>> GetActivePrintFormatsIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activePrintFormatIds));

    public Task<IReadOnlySet<int>> GetActiveContactTypeIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeContactTypeIds));

    public Task<IReadOnlySet<int>> GetActiveUnitTypesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeCurrencyIds));

    public Task<IReadOnlySet<int>> GetActiveAccommodationRoomTypesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeAccommodationRoomTypeIds));

    public Task<IReadOnlySet<int>> GetActiveAccommodationBoardTypesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeAccommodationBoardTypeIds));

    public Task<IReadOnlySet<int>> GetActiveAccommodationBathroomTypesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeAccommodationBathroomTypeIds));

    public Task<IReadOnlySet<int>> GetActiveAccommodationRoomGradesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default)
        => Task.FromResult(Filter(ids, _activeAccommodationRoomGradeIds));

    public Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(IReadOnlyCollection<int> ids, ContentTemplateScope scope, CancellationToken ct = default)
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
