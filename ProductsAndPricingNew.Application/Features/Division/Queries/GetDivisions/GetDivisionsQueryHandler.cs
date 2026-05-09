using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

internal sealed class GetDivisionsQueryHandler : IRequestHandler<GetDivisionsQuery, Result<PagedResult<DivisionListItemDto>>>
{
    private readonly IDivisionQuery _divisionQuery;

    public GetDivisionsQueryHandler(IDivisionQuery divisionQuery)
    {
        _divisionQuery = divisionQuery;
    }

    public async Task<Result<PagedResult<DivisionListItemDto>>> Handle(GetDivisionsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<DivisionListItemDto> result = await _divisionQuery.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}
