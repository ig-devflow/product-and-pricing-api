using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Abstractions;

public interface IDivisionCommandPayload
{
    string Name { get; }
    string WebsiteUrl { get; }
    string? TermsAndConditions { get; }
    string? GroupsPaymentTerms { get; }
    string? HeadOfficeEmail { get; }
    string? HeadOfficeTelephoneNo { get; }
    AddressDto? ContactAddress { get; }
    ImageFileDto? AccreditationBanner { get; }
    IReadOnlyCollection<TextContentDto> Texts { get; }
}