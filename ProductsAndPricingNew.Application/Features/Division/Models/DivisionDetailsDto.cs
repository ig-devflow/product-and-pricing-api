using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionDetailsDto(
    int Id,
    string Name,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? WebsiteUrl,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    ImageBannerDto? ImageBanner,
    AddressDto? ContactAddress
);