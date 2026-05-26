using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodations;

internal sealed class GetAccommodationsQueryHandler : IRequestHandler<GetAccommodationsQuery, Result<PagedResult<AccommodationListItemDto>>>
{
    private readonly IAccommodationQuery _accommodationQuery;

    public GetAccommodationsQueryHandler(IAccommodationQuery accommodationQuery)
    {
        _accommodationQuery = accommodationQuery;
    }

    public async Task<Result<PagedResult<AccommodationListItemDto>>> Handle(GetAccommodationsQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<AccommodationListItemDto> result = await _accommodationQuery.GetListAsync(
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}