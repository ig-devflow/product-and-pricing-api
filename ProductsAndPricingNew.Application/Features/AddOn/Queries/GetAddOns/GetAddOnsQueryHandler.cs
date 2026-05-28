using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOns;

internal sealed class GetAddOnsQueryHandler : IRequestHandler<GetAddOnsQuery, Result<PagedResult<AddOnListItemDto>>>
{
    private readonly IAddOnQuery _addOnQuery;

    public GetAddOnsQueryHandler(IAddOnQuery addOnQuery)
    {
        _addOnQuery = addOnQuery;
    }

    public async Task<Result<PagedResult<AddOnListItemDto>>> Handle(GetAddOnsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<AddOnListItemDto> result = await _addOnQuery.GetListAsync(
            request.DivisionId,
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}
