using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRoomById;

internal sealed class GetAccommodationRoomByIdQueryHandler : IRequestHandler<GetAccommodationRoomByIdQuery, Result<AccommodationRoomDetailsDto>>
{
    private readonly IAccommodationRoomQuery _accommodationRoomQuery;

    public GetAccommodationRoomByIdQueryHandler(IAccommodationRoomQuery accommodationRoomQuery)
    {
        _accommodationRoomQuery = accommodationRoomQuery;
    }

    public async Task<Result<AccommodationRoomDetailsDto>> Handle(GetAccommodationRoomByIdQuery request, CancellationToken ct)
    {
        AccommodationRoomDetailsDto? result = await _accommodationRoomQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Accommodation room with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}