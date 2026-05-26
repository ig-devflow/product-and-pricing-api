using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Common.Validation.Abstractions;

public interface IReferenceDataValidationQuery
{
    Task<IReadOnlySet<int>> GetActiveCountryIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveAudienceIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(IReadOnlyCollection<int> ids, ContentTemplateScope scope, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveCurrencyIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActivePrintFormatsIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveContactTypeIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveUnitTypesIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
}
