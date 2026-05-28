using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Models;

namespace ProductsAndPricingNew.Application.Features.Course.Queries.GetCourses;

public sealed record GetCoursesQuery(
    int DivisionId,
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<CourseListItemDto>>>;
