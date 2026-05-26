namespace ProductsAndPricingNew.Application.Features.Accommodation.Models;

public record AccommodationListItemDto(
    int Id,
    string Name,
    string AccommodationTypeName,
    bool IsActive,
    DateOnly CreatedAt,
    string? CreatedByName,
    DateOnly UpdatedAt,
    string? UpdatedByName
);