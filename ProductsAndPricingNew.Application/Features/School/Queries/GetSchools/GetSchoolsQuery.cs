using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchools;

public sealed record GetSchoolsQuery(
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default,
    int? CentreId = null
) : IRequest<Result<PagedResult<SchoolListItemDto>>>;