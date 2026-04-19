using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

public sealed record CreateDivisionCommand(
    string Name,
    bool ShowInDropdown,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? WebsiteUrl,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    DivisionAddressDto? ContactAddress
) : ICommand<int>;