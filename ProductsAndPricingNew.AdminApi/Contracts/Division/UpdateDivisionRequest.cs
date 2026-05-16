using ProductsAndPricingNew.AdminApi.Contracts.Common;

namespace ProductsAndPricingNew.AdminApi.Contracts.Division;

public sealed record UpdateDivisionRequest(
    string Name,
    string WebsiteUrl,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    AddressRequest? ContactAddress,
    ImageFileRequest? AccreditationBanner,
    IReadOnlyCollection<TextContentRequest> Texts,
    string Version
);