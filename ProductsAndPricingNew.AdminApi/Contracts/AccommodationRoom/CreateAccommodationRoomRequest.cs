namespace ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;

public sealed record CreateAccommodationRoomRequest(
    string Name,
    int UnitTypeId,
    bool IsActive,
    bool OccupyRoom,
    RoomDetailsRequest RoomDetails,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
);