using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;

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
    AddressDto? ContactAddress,
    ImageFileDto? AccreditationBanner,
    IReadOnlyCollection<TextContentDto> Texts,
    string Version
) : ICommand<Result<Unit>>, IDivisionCommandPayload;