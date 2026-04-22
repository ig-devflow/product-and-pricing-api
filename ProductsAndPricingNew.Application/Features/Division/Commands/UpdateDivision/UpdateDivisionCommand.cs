using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

public sealed record UpdateDivisionCommand(
    int Id,
    string Name,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? WebsiteUrl,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    DivisionAddressDto? ContactAddress
) : ICommand<Unit>;