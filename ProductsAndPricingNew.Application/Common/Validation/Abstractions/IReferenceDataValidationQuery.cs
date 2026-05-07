using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Application.Common.Validation.Abstractions;

public interface IReferenceDataValidationQuery
{
    Task<IReadOnlySet<int>> GetExistingCountryIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveAudienceIdsAsync(IReadOnlyCollection<int> ids, CancellationToken ct = default);
    Task<IReadOnlySet<int>> GetActiveContentTemplateIdsAsync(IReadOnlyCollection<int> ids, ContentTemplateScope scope, CancellationToken ct = default);
}