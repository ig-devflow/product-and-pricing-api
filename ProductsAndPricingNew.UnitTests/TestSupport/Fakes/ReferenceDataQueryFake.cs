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
    private IReadOnlyCollection<UnitTypeReferenceDto> _unitTypes = [];
    private IReadOnlyCollection<CentreContactTypeReferenceDto> _centreContactTypes = [];
    private IReadOnlyCollection<PrintFormatReferenceDto> _printFormats = [];
    private IReadOnlyCollection<AccommodationRoomTypeReferenceDto> _accommodationRoomTypes = [];
    private IReadOnlyCollection<AccommodationBoardTypeReferenceDto> _accommodationBoardTypes = [];
    private IReadOnlyCollection<AccommodationBathroomTypeReferenceDto> _accommodationBathroomTypes = [];
    private IReadOnlyCollection<AccommodationRoomGradeReferenceDto> _accommodationRoomGrades = [];

    public int GetCountriesCalls { get; private set; }
    public int GetCurrenciesCalls { get; private set; }
    public int GetAudiencesCalls { get; private set; }
    public int GetContentTemplatesCalls { get; private set; }
    public int GetUnitTypesCalls { get; private set; }
    public int GetCentreContactTypeCalls { get; private set; }
    public int GetPrintFormatsCalls { get; private set; }
    public int GetAccommodationRoomTypesCalls { get; private set; }
    public int GetAccommodationBoardTypesCalls { get; private set; }
    public int GetAccommodationBathroomTypesCalls { get; private set; }
    public int GetAccommodationRoomGradesCalls { get; private set; }
    public ContentTemplateScope? LastContentTemplateScope { get; private set; }

    #region Builder

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

    public ReferenceDataQueryFake WithCentreContactTypes(IReadOnlyCollection<CentreContactTypeReferenceDto> centreContactTypes)
    {
        _centreContactTypes = centreContactTypes;
        return this;
    }

    public ReferenceDataQueryFake WithPrintFormats(IReadOnlyCollection<PrintFormatReferenceDto> printFormats)
    {
        _printFormats = printFormats;
        return this;
    }

    public ReferenceDataQueryFake WithContentTemplates(IReadOnlyCollection<ContentTemplateReferenceDto> contentTemplates)
    {
        _contentTemplates = contentTemplates;
        return this;
    }

    public ReferenceDataQueryFake WithUnitTypes(IReadOnlyCollection<UnitTypeReferenceDto> unitTypes)
    {
        _unitTypes = unitTypes;
        return this;
    }

    public ReferenceDataQueryFake WithRoomTypes(IReadOnlyCollection<AccommodationRoomTypeReferenceDto> roomTypes)
    {
        _accommodationRoomTypes = roomTypes;
        return this;
    }

    public ReferenceDataQueryFake WithBoardTypes(IReadOnlyCollection<AccommodationBoardTypeReferenceDto> boardTypes)
    {
        _accommodationBoardTypes = boardTypes;
        return this;
    }

    public ReferenceDataQueryFake WithBathroomTypes(IReadOnlyCollection<AccommodationBathroomTypeReferenceDto> bathroomTypes)
    {
        _accommodationBathroomTypes = bathroomTypes;
        return this;
    }

    public ReferenceDataQueryFake WithRoomGrades(IReadOnlyCollection<AccommodationRoomGradeReferenceDto> roomGrades)
    {
        _accommodationRoomGrades = roomGrades;
        return this;
    }
    #endregion

    #region AsyncMethods

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

    public Task<IReadOnlyCollection<CentreContactTypeReferenceDto>> GetCentreContactTypesAsync(CancellationToken ct = default)
    {
        GetCentreContactTypeCalls++;
        return Task.FromResult(_centreContactTypes);
    }

    public Task<IReadOnlyCollection<UnitTypeReferenceDto>> GetUnitTypesAsync(CancellationToken ct = default)
    {
        GetUnitTypesCalls++;
        return Task.FromResult(_unitTypes);
    }

    public Task<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>> GetAccommodationRoomTypesAsync(CancellationToken ct = default)
    {
        GetAccommodationRoomTypesCalls++;
        return Task.FromResult(_accommodationRoomTypes);
    }

    public Task<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>> GetAccommodationBoardTypesAsync(CancellationToken ct = default)
    {
        GetAccommodationBoardTypesCalls++;
        return Task.FromResult(_accommodationBoardTypes);
    }

    public Task<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>> GeAccommodationBathroomTypesAsync(CancellationToken ct = default)
    {
        GetAccommodationBathroomTypesCalls++;
        return Task.FromResult(_accommodationBathroomTypes);
    }

    public Task<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>> GetAccommodationRoomGradesAsync(CancellationToken ct = default)
    {
        GetAccommodationRoomGradesCalls++;
        return Task.FromResult(_accommodationRoomGrades);
    }

    public Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(ContentTemplateScope? scope, CancellationToken ct = default)
    {
        GetContentTemplatesCalls++;
        LastContentTemplateScope = scope;
        return Task.FromResult(_contentTemplates);
    }

    public Task<IReadOnlyCollection<PrintFormatReferenceDto>> GetPrintFormatsAsync(CancellationToken ct = default)
    {
        GetPrintFormatsCalls++;
        return Task.FromResult(_printFormats);
    }
    #endregion
}
