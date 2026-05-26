using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;

internal interface IAccommodationRoomCommandPayload
{
    string Name { get; }
    int UnitTypeId { get; }
    bool IsActive { get; }
    bool OccupyRoom { get; }
    RoomDetailsDto RoomDetails { get; }
    int AccountCategoryId { get; }
    int ProductCategoryId { get; }
    string? GeneralLedgerCode { get; }
    string? CostCentreCode { get; }
    DateOnly? ClosurePolicy { get; }
}