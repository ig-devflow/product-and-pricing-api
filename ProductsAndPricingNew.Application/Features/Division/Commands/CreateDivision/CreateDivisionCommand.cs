using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;

public sealed record CreateDivisionCommand(
    string Name,
    string WebsiteUrl,
    bool IsActive,
    string? TermsAndConditions,
    string? GroupsPaymentTerms,
    string? HeadOfficeEmail,
    string? HeadOfficeTelephoneNo,
    AddressDto? ContactAddress,
    ImageBannerDto? AccreditationBanner
) : ICommand<Result<int>>;