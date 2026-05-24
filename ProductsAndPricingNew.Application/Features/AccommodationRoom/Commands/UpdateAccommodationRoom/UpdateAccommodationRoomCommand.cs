using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.UpdateAccommodationRoom;

public sealed record UpdateAccommodationRoomCommand(
    int Id,
    string Name,
    int UnitTypeId,
    bool IsActive,
    bool OccupyRoom,
    RoomDetailsDto roomDetails,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version
) : ICommand<Result<Unit>>;