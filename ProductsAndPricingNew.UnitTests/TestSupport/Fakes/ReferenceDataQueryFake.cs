using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class ReferenceDataQueryFake : IReferenceDataQuery
{
    private IReadOnlyCollection<CountryReferenceDto> _countries = [];
    private IReadOnlyCollection<CurrencyReferenceDto> _currencies = [];
    private IReadOnlyCollection<AudienceReferenceDto> _audiences = [];
    private IReadOnlyCollection<ContentTemplateReferenceDto> _contentTemplates = [];

    public int GetCountriesCalls { get; private set; }
    public int GetCurrenciesCalls { get; private set; }
    public int GetAudiencesCalls { get; private set; }
    public int GetContentTemplatesCalls { get; private set; }
    public ContentTemplateScope? LastContentTemplateScope { get; private set; }

    public ReferenceDataQueryFake WithCountries(IReadOnlyCollection<CountryReferenceDto> countries)
    {
        _countries = countries;
        return this;
    }

    public ReferenceDataQueryFake WithCurrencies(IReadOnlyCollection<CurrencyReferenceDto> currencies)
    {
        _currencies = currencies;
        return this;
    }

    public ReferenceDataQueryFake WithAudiences(IReadOnlyCollection<AudienceReferenceDto> audiences)
    {
        _audiences = audiences;
        return this;
    }

    public ReferenceDataQueryFake WithContentTemplates(IReadOnlyCollection<ContentTemplateReferenceDto> contentTemplates)
    {
        _contentTemplates = contentTemplates;
        return this;
    }

    public Task<IReadOnlyCollection<CountryReferenceDto>> GetCountriesAsync(CancellationToken ct = default)
    {
        GetCountriesCalls++;
        return Task.FromResult(_countries);
    }

    public Task<IReadOnlyCollection<CurrencyReferenceDto>> GetCurrenciesAsync(CancellationToken ct = default)
    {
        GetCurrenciesCalls++;
        return Task.FromResult(_currencies);
    }

    public Task<IReadOnlyCollection<AudienceReferenceDto>> GetAudiencesAsync(CancellationToken ct = default)
    {
        GetAudiencesCalls++;
        return Task.FromResult(_audiences);
    }

    public Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(
        ContentTemplateScope? scope,
        CancellationToken ct = default)
    {
        GetContentTemplatesCalls++;
        LastContentTemplateScope = scope;
        return Task.FromResult(_contentTemplates);
    }
}
