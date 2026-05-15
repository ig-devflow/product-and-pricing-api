namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreLegalInfoDto(
    string? SchoolSponsorshipNumber,
    string? VatNumber,
    string? RegistrationNumber,
    string? VatExemptionNumber,
    string? ChequePayableTo
);