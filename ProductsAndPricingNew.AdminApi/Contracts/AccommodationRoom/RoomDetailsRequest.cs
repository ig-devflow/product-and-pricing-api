namespace ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;

public sealed record RoomDetailsRequest(
    int RoomTypeId,
    int BoardTypeId,
    int BathroomTypeId,
    int RoomGradeId
);