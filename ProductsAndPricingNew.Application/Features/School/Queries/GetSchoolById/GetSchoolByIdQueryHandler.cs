using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Models;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchoolById;

internal sealed class GetSchoolByIdQueryHandler : IRequestHandler<GetSchoolByIdQuery, Result<SchoolDetailsDto>>
{
    private readonly ISchoolQuery _schoolQuery;

    public GetSchoolByIdQueryHandler(ISchoolQuery schoolQuery)
    {
        _schoolQuery = schoolQuery;
    }

    public async Task<Result<SchoolDetailsDto>> Handle(GetSchoolByIdQuery request, CancellationToken ct)
    {
        SchoolDetailsDto? result = await _schoolQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"School with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}