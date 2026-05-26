namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

public sealed record AccommodationRoomDetailsDto(
    int Id,
    int AccommodationId,
    int DivisionId,
    int UnitTypeId,
    string Name,
    bool IsActive,
    bool OccupyRoom,
    RoomDetailsDto RoomDetails,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);