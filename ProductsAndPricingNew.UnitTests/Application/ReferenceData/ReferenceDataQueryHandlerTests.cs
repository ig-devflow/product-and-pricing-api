using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAudiences;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCentreContactTypes;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetContentTemplates;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCountries;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCurrencies;
using ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetPrintFormats;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.UnitTests.TestSupport.Assertions;
using ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

namespace ProductsAndPricingNew.UnitTests.Application.ReferenceData;

public sealed class ReferenceDataQueryHandlerTests
{
    [Fact]
    public async Task GetCountries_DelegatesToReferenceDataQuery_AndReturnsOk()
    {
        CountryReferenceDto[] countries = [new(1, "US", "United States")];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithCountries(countries);
        GetCountriesQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetCountriesQuery(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(countries, result.Value);
        Assert.Equal(1, referenceDataQuery.GetCountriesCalls);
    }

    [Fact]
    public async Task GetCurrencies_DelegatesToReferenceDataQuery_AndReturnsOk()
    {
        CurrencyReferenceDto[] currencies = [new(1, "USD", "US Dollar", "$")];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithCurrencies(currencies);
        GetCurrenciesQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetCurrenciesQuery(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(currencies, result.Value);
        Assert.Equal(1, referenceDataQuery.GetCurrenciesCalls);
    }

    [Fact]
    public async Task GetAudiences_DelegatesToReferenceDataQuery_AndReturnsOk()
    {
        AudienceReferenceDto[] audiences = [new(1, "Students")];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithAudiences(audiences);
        GetAudiencesQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetAudiencesQuery(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(audiences, result.Value);
        Assert.Equal(1, referenceDataQuery.GetAudiencesCalls);
    }

    [Fact]
    public async Task GetContentTemplates_DelegatesToReferenceDataQuery_WithScope_AndReturnsOk()
    {
        ContentTemplateReferenceDto[] contentTemplates =
        [
            new(1, "Terms", "Division terms", ContentTemplateScope.Division)
        ];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithContentTemplates(contentTemplates);
        GetContentTemplatesQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetContentTemplatesQuery(ContentTemplateScope.Division), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(contentTemplates, result.Value);
        Assert.Equal(1, referenceDataQuery.GetContentTemplatesCalls);
        Assert.Equal(ContentTemplateScope.Division, referenceDataQuery.LastContentTemplateScope);
    }

    [Fact]
    public async Task CentreContactTypes_DelegatesToReferenceDataQuery_AndReturnsOk()
    {
        CentreContactTypeReferenceDto[] contactTypes =
        [
            new(1, "Contact 1")
        ];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithCentreContactTypes(contactTypes);
        GetCentreContactTypesQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetCentreContactTypesQuery(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(contactTypes, result.Value);
        Assert.Equal(1, referenceDataQuery.GetCentreContactTypeCalls);
    }

    [Fact]
    public async Task PrintFormats_DelegatesToReferenceDataQuery_AndReturnsOk()
    {
        PrintFormatReferenceDto[] formats =
        [
            new(1, "A4")
        ];
        ReferenceDataQueryFake referenceDataQuery = new ReferenceDataQueryFake()
            .WithPrintFormats(formats);
        GetPrintFormatsQueryHandler handler = new(referenceDataQuery);

        var result = await handler.Handle(new GetPrintFormatsQuery(), CancellationToken.None);

        ResultAssertions.AssertSucceeded(result);
        Assert.Same(formats, result.Value);
        Assert.Equal(1, referenceDataQuery.GetPrintFormatsCalls);
    }
}
