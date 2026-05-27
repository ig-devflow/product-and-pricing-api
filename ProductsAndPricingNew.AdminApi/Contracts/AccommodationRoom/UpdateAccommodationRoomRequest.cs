namespace ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;

public sealed record UpdateAccommodationRoomRequest(
    string Name,
    int UnitTypeId,
    bool IsActive,
    bool OccupyRoom,
    RoomDetailsRequest RoomDetails,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version
);