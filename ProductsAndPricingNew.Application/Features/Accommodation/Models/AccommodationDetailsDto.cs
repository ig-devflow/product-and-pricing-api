namespace ProductsAndPricingNew.Application.Features.Accommodation.Models;

public record AccommodationDetailsDto(
    int Id,
    string Name,
    int AccommodationTypeId,
    bool IsActive,
    int MinimumStayInWeeks,
    int? MinimumAge,
    int? MaximumAge,
    bool IsCommitted,
    bool IsNonCommitted,
    string Version,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);