using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

public sealed record GetDivisionsQuery(
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<PagedResult<DivisionListItemDto>>;