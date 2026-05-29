using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentreOptions;

internal sealed class GetCentreOptionsQueryHandler : IRequestHandler<GetCentreOptionsQuery, Result<IReadOnlyCollection<CentreOptionDto>>>
{
    private readonly ICentreQuery _centreQuery;

    public GetCentreOptionsQueryHandler(ICentreQuery centreQuery)
    {
        _centreQuery = centreQuery;
    }

    public async Task<Result<IReadOnlyCollection<CentreOptionDto>>> Handle(GetCentreOptionsQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CentreOptionDto> result = await _centreQuery.GetOptionsAsync(ct);
        return Result.Ok(result);
    }
}
