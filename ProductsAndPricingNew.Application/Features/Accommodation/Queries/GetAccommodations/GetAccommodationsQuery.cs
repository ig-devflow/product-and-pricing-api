using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodations;

public sealed record GetAccommodationsQuery(
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<AccommodationListItemDto>>>;