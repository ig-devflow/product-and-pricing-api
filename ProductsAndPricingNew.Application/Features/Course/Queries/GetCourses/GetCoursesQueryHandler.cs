using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Course.Queries.GetCourses;

internal sealed class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, Result<PagedResult<CourseListItemDto>>>
{
    private readonly ICourseQuery _courseQuery;

    public GetCoursesQueryHandler(ICourseQuery courseQuery)
    {
        _courseQuery = courseQuery;
    }

    public async Task<Result<PagedResult<CourseListItemDto>>> Handle(GetCoursesQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<CourseListItemDto> result = await _courseQuery.GetListAsync(
            request.DivisionId,
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}
