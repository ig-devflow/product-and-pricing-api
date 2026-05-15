using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentres;

public sealed record GetCentresQuery(
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<CentreListItemDto>>>;