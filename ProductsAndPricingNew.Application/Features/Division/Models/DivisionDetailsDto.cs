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
    ImageFileDto? AccreditationBanner,
    AddressDto? ContactAddress,
    IReadOnlyCollection<DivisionTextContentDto> Texts,
    string Version,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);