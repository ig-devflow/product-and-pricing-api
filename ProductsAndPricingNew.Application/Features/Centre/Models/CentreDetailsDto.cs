using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreDetailsDto(
    int Id,
    string Name,
    string Code,
    int CurrencyId,
    PrintFormat PrintFormat,
    bool IsActive,
    bool IsPhysicalCentre,
    CentreContactInfoDto ContactInfo,
    CentreLegalInfoDto LegalInfo,
    CentreOperationalRatiosDto OperationalRatios,
    CentreBankDetailsDto BankDetails,
    IReadOnlyCollection<CentreContactDto> Contacts,
    IReadOnlyCollection<CentreTextContentDto> Texts,
    string Version,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);