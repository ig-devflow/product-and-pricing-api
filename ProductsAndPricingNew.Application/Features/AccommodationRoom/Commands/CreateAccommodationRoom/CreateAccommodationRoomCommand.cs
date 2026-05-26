using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.CreateAccommodationRoom;

public sealed record CreateAccommodationRoomCommand(
    int AccommodationId,
    int DivisionId,
    string Name,
    int UnitTypeId,
    bool IsActive,
    bool OccupyRoom,
    RoomDetailsDto RoomDetails,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
) : IRequest<Result<int>>, IAccommodationRoomCommandPayload;