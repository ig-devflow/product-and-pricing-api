using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

internal sealed class GetDivisionsQueryHandler : IRequestHandler<GetDivisionsQuery, Result<PagedResult<DivisionListItemDto>>>
{
    private readonly IDivisionQueries _divisionQueries;

    public GetDivisionsQueryHandler(IDivisionQueries divisionQueries)
    {
        _divisionQueries = divisionQueries;
    }

    public async Task<Result<PagedResult<DivisionListItemDto>>> Handle(GetDivisionsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = !string.IsNullOrWhiteSpace(request.Search)
            ? request.Search.Trim()
            : null;

        PagedResult<DivisionListItemDto> result = await _divisionQueries.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}