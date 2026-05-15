using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreContactInfoDto(
    string? GeneralEmail,
    string? AccommodationEmail,
    string? Telephone,
    string? EmergencyTelephone,
    string? TransferEmergencyTelephone,
    string? BrandColor,
    AddressDto? ContactAddress,
    ImageFileDto? LogoImage
);