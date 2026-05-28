using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Models;

namespace ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOns;

public sealed record GetAddOnsQuery(
    int DivisionId,
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<AddOnListItemDto>>>;
