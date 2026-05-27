using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Abstractions;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRooms;

internal sealed class GetAccommodationRoomsQueryHandler : IRequestHandler<GetAccommodationRoomsQuery, Result<PagedResult<AccommodationRoomListItemDto>>>
{
    private readonly IAccommodationRoomQuery _accommodationRoomQuery;

    public GetAccommodationRoomsQueryHandler(IAccommodationRoomQuery accommodationRoomQuery)
    {
        _accommodationRoomQuery = accommodationRoomQuery;
    }

    public async Task<Result<PagedResult<AccommodationRoomListItemDto>>> Handle(GetAccommodationRoomsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<AccommodationRoomListItemDto> result = await _accommodationRoomQuery.GetListAsync(
            request.DivisionId,
            request.AccommodationId,
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}