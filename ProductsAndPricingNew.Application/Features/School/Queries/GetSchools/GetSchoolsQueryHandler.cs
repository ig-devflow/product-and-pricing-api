using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.School.Queries.GetSchools;

internal sealed class GetSchoolsQueryHandler : IRequestHandler<GetSchoolsQuery, Result<PagedResult<SchoolListItemDto>>>
{
    private readonly ISchoolQuery _schoolQuery;

    public GetSchoolsQueryHandler(ISchoolQuery schoolQuery)
    {
        _schoolQuery = schoolQuery;
    }

    public async Task<Result<PagedResult<SchoolListItemDto>>> Handle(GetSchoolsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<SchoolListItemDto> result = await _schoolQuery.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            request.CentreId,
            ct);

        return Result.Ok(result);
    }
}