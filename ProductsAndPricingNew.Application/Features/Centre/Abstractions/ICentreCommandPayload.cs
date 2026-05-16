using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.Centre.Abstractions;

public interface ICentreCommandPayload
{
    string Name { get; }
    string Code { get; }
    int CurrencyId { get; }
    PrintFormat PrintFormat { get; }
    bool IsActive { get; }
    bool IsPhysicalCentre { get; }
    CentreContactInfoDto ContactInfo { get; }
    CentreLegalInfoDto LegalInfo { get; }
    CentreOperationalRatiosDto OperationalRatios { get; }
    CentreBankDetailsDto BankDetails { get; }
    IReadOnlyCollection<CentreContactDto> Contacts { get; }
    IReadOnlyCollection<TextContentDto> Texts { get; }
}
