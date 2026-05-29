using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCourseLanguages;

internal sealed class GetCourseLanguagesQueryHandler : IRequestHandler<GetCourseLanguagesQuery, Result<IReadOnlyCollection<CourseLanguageReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetCourseLanguagesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<CourseLanguageReferenceDto>>> Handle(GetCourseLanguagesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CourseLanguageReferenceDto> result = await _referenceDataQuery.GetCourseLanguagesAsync(ct);
        return Result.Ok(result);
    }
}
