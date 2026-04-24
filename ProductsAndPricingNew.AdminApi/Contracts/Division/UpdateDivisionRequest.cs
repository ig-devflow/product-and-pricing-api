using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.AdminApi.Contracts.Division;

public sealed record UpdateDivisionRequest(
    string Name,
    string WebsiteUrl,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    DivisionAddressDto? ContactAddress,
    DivisionAccreditationBanner? AccreditationBanner
);