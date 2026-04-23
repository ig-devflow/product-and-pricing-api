using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Dtos;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

internal sealed class GetDivisionsQueryHandler : IRequestHandler<GetDivisionsQuery, PagedResult<DivisionListItemDto>>
{
    private readonly IDivisionQueries _divisionQueries;

    public GetDivisionsQueryHandler(IDivisionQueries divisionQueries)
    {
        _divisionQueries = divisionQueries;
    }

    public Task<PagedResult<DivisionListItemDto>> Handle(GetDivisionsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = !string.IsNullOrWhiteSpace(request.Search)
            ? request.Search.Trim()
            : null;

        return _divisionQueries.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);
    }
}