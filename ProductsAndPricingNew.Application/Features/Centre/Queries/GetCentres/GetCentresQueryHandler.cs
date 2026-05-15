using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentres;

internal sealed class GetCentresQueryHandler : IRequestHandler<GetCentresQuery, Result<PagedResult<CentreListItemDto>>>
{
    private readonly ICentreQuery _centreQuery;

    public GetCentresQueryHandler(ICentreQuery centreQuery)
    {
        _centreQuery = centreQuery;
    }

    public async Task<Result<PagedResult<CentreListItemDto>>> Handle(GetCentresQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<CentreListItemDto> result = await _centreQuery.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}