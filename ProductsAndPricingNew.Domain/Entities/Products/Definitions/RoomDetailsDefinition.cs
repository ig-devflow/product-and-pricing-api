namespace ProductsAndPricingNew.Domain.Entities.Products.Definitions;

public sealed record RoomDetailsDefinition(
    int RoomTypeId,
    int BoardTypeId,
    int BathroomTypeId,
    int RoomGradeId
);