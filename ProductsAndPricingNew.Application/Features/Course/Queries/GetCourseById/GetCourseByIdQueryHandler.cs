using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Models;

namespace ProductsAndPricingNew.Application.Features.Course.Queries.GetCourseById;

internal sealed class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDetailsDto>>
{
    private readonly ICourseQuery _courseQuery;

    public GetCourseByIdQueryHandler(ICourseQuery courseQuery)
    {
        _courseQuery = courseQuery;
    }

    public async Task<Result<CourseDetailsDto>> Handle(GetCourseByIdQuery request, CancellationToken ct)
    {
        CourseDetailsDto? result = await _courseQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Course with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
