using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.Centre.Commands.CreateCentre;

public sealed record CreateCentreCommand(
    string Name,
    string Code,
    int CurrencyId,
    PrintFormat PrintFormat,
    bool IsActive,
    bool IsPhysicalCentre,
    CentreContactInfoDto ContactInfo,
    CentreLegalInfoDto LegalInfo,
    CentreOperationalRatiosDto OperationalRatios,
    CentreBankDetailsDto BankDetails,
    IReadOnlyCollection<CentreContactDto> Contacts,
    IReadOnlyCollection<TextContentDto> Texts
) : ICommand<Result<int>>, ICentreCommandPayload;