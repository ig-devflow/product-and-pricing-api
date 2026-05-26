using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRooms;

public record GetAccommodationRoomsQuery(
    int DivisionId,
    int AccommodationId,
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<AccommodationRoomListItemDto>>>;