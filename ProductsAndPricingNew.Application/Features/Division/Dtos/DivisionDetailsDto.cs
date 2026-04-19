namespace ProductsAndPricingNew.Application.Features.Division.Dtos;

public sealed record DivisionDetailsDto(
    int Id,
    string Name,
    bool ShowInDropdown,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? WebsiteUrl,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    DivisionAddressDto? ContactAddress
);