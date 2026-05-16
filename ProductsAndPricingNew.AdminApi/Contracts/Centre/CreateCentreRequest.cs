using ProductsAndPricingNew.AdminApi.Contracts.Common;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CreateCentreRequest(
    string Name,
    string Code,
    int CurrencyId,
    PrintFormat PrintFormat,
    bool IsActive,
    bool IsPhysicalCentre,
    CentreContactInfoRequest ContactInfo,
    CentreLegalInfoRequest LegalInfo,
    CentreOperationalRatiosRequest OperationalRatios,
    CentreBankDetailsRequest BankDetails,
    IReadOnlyCollection<CentreContactRequest> Contacts,
    IReadOnlyCollection<TextContentRequest> Texts
);