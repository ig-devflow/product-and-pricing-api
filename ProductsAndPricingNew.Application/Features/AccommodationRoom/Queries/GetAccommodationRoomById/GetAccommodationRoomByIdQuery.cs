using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRoomById;

public sealed record GetAccommodationRoomByIdQuery(int Id) : IRequest<Result<AccommodationRoomDetailsDto>>;