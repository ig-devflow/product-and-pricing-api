namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

public record AccommodationRoomListItemDto(
    int Id,
    string AccommodationName,
    string DivisionName,
    string Name,
    bool IsActive,
    bool OccupyRoom,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);