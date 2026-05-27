using ProductsAndPricingNew.Application.Features.ReferenceData.Models;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;

public interface IReferenceDataQuery
{
    Task<IReadOnlyCollection<CountryReferenceDto>> GetCountriesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<CurrencyReferenceDto>> GetCurrenciesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AudienceReferenceDto>> GetAudiencesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<ContentTemplateReferenceDto>> GetContentTemplatesAsync(ContentTemplateScope? scope, CancellationToken ct = default);
    Task<IReadOnlyCollection<PrintFormatReferenceDto>> GetPrintFormatsAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<CentreContactTypeReferenceDto>> GetCentreContactTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<UnitTypeReferenceDto>> GetUnitTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AccommodationTypeReferenceDto>> GetAccommodationTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>> GetAccommodationRoomTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>> GetAccommodationBoardTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>> GetAccommodationBathroomTypesAsync(CancellationToken ct = default);
    Task<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>> GetAccommodationRoomGradesAsync(CancellationToken ct = default);
}
