using ProductsAndPricingNew.AdminApi.Contracts.Common;
using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.AdminApi.Contracts.Division;

public sealed record CreateDivisionRequest(
    string Name,
    string WebsiteUrl,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    AddressRequest? ContactAddress,
    ImageBannerRequest? AccreditationBanner,
    IReadOnlyCollection<TextContentRequest> Texts
);