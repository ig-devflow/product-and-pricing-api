using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.School.Models;

public sealed record SchoolDetailsDto(
    int Id,
    int CentreId,
    string Name,
    string LegacyCode,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    string? Telephone,
    string? EmergencyTelephone,
    AddressDto? ContactAddress,
    string FinanceCode,
    bool LmsAccess,
    bool IsActive,
    DateOnly? DecommissionDate,
    string Version,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);