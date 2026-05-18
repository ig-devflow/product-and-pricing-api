using ProductsAndPricingNew.AdminApi.Contracts.Common;

namespace ProductsAndPricingNew.AdminApi.Contracts.School;

public sealed record CreateSchoolRequest(
    string Name,
    string LegacyCode,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    string? Telephone,
    string? EmergencyTelephone,
    AddressRequest? ContactAddress,
    string? FinanceCode,
    bool LmsAccess,
    bool IsActive,
    DateOnly? DecommissionDate
);