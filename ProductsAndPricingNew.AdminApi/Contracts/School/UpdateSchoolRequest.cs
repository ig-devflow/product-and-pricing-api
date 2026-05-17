using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.AdminApi.Contracts.School;

public record UpdateSchoolRequest(
    string Name,
    string LegacyCode,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    string? Telephone,
    string? EmergencyTelephone,
    AddressDto? ContactAddress,
    string? FinanceCode,
    bool LmsAccess,
    bool IsActive,
    DateOnly? DecommissionDate,
    string Version
);