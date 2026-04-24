using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

public sealed record UpdateDivisionCommand(
    int Id,
    string Name,
    string WebsiteUrl,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    DivisionAddressDto? ContactAddress,
    DivisionAccreditationBanner? AccreditationBanner
) : ICommand<Result<Unit>>;