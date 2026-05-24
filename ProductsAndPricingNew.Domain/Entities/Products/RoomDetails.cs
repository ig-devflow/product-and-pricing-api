using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.Products.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public readonly record struct RoomDetails : IEmptyValueObject
{
    public int RoomTypeId { get; }
    public int BoardTypeId { get; }
    public int BathroomTypeId { get; }
    public int RoomGradeId { get; }

    public bool IsEmpty => RoomTypeId <= 0 &&
                           BoardTypeId <= 0 &&
                           BathroomTypeId <= 0 &&
                           RoomGradeId <= 0;

    public static readonly RoomDetails Unassigned = new(0, 0, 0, 0);

    private RoomDetails(int roomTypeId, int boardTypeId, int bathroomTypeId, int roomGradeId)
    {
        RoomTypeId = roomTypeId;
        BoardTypeId = boardTypeId;
        BathroomTypeId = bathroomTypeId;
        RoomGradeId = roomGradeId;
    }

    public static RoomDetails Create(RoomDetailsDefinition roomDetails)
    {
        Guard.PositiveId(roomDetails.RoomTypeId, nameof(RoomTypeId));
        Guard.PositiveId(roomDetails.BoardTypeId, nameof(BoardTypeId));
        Guard.PositiveId(roomDetails.BathroomTypeId, nameof(BathroomTypeId));
        Guard.PositiveId(roomDetails.RoomGradeId, nameof(RoomGradeId));

        return new RoomDetails(roomDetails.RoomTypeId, roomDetails.BoardTypeId, roomDetails.BathroomTypeId, roomDetails.RoomGradeId);
    }
}