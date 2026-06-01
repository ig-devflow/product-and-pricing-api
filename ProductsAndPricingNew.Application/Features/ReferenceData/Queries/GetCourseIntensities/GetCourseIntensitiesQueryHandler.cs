using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCourseIntensities;

internal sealed class GetCourseIntensitiesQueryHandler : IRequestHandler<GetCourseIntensitiesQuery, Result<IReadOnlyCollection<CourseIntensityReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetCourseIntensitiesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<CourseIntensityReferenceDto>>> Handle(GetCourseIntensitiesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CourseIntensityReferenceDto> result = await _referenceDataQuery.GetCourseIntensitiesAsync(ct);
        return Result.Ok(result);
    }
}
