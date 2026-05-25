namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

public sealed record RoomDetailsDto(
    int RoomTypeId,
    int BoardTypeId,
    int BathroomTypeId,
    int RoomGradeId
);