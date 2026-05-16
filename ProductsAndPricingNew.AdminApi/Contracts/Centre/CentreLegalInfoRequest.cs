namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreLegalInfoRequest(
    string? SchoolSponsorshipNumber,
    string? VatNumber,
    string? RegistrationNumber,
    string? VatExemptionNumber,
    string? ChequePayableTo
);