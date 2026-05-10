using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;

public interface IReferenceDataQuery
{
    Task<IReadOnlyCollection<CountryReferenceDto>> GetCountriesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<CurrencyReferenceDto>> GetCurrenciesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AudienceReferenceDto>> GetAudiencesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(ContentTemplateScope? scope, CancellationToken ct = default);
}
