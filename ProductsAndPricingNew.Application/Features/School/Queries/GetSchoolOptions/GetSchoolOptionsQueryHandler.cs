using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchoolOptions;

internal sealed class GetSchoolOptionsQueryHandler : IRequestHandler<GetSchoolOptionsQuery, Result<IReadOnlyCollection<SchoolOptionDto>>>
{
    private readonly ISchoolQuery _schoolQuery;

    public GetSchoolOptionsQueryHandler(ISchoolQuery schoolQuery)
    {
        _schoolQuery = schoolQuery;
    }

    public async Task<Result<IReadOnlyCollection<SchoolOptionDto>>> Handle(GetSchoolOptionsQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<SchoolOptionDto> result = await _schoolQuery.GetOptionsAsync(request.CentreId, ct);
        return Result.Ok(result);
    }
}
