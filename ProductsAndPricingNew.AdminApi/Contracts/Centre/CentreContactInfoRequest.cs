using ProductsAndPricingNew.AdminApi.Contracts.Common;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreContactInfoRequest(
    string? GeneralEmail,
    string? AccommodationEmail,
    string? Telephone,
    string? EmergencyTelephone,
    string? TransferEmergencyTelephone,
    string? BrandColor,
    AddressRequest? ContactAddress,
    ImageFileRequest? LogoImage
);