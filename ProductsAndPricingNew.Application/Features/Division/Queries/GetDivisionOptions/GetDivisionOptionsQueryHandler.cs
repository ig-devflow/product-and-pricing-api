using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionOptions;

internal sealed class GetDivisionOptionsQueryHandler : IRequestHandler<GetDivisionOptionsQuery, Result<IReadOnlyCollection<DivisionOptionDto>>>
{
    private readonly IDivisionQuery _divisionQuery;

    public GetDivisionOptionsQueryHandler(IDivisionQuery divisionQuery)
    {
        _divisionQuery = divisionQuery;
    }

    public async Task<Result<IReadOnlyCollection<DivisionOptionDto>>> Handle(GetDivisionOptionsQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<DivisionOptionDto> result = await _divisionQuery.GetOptionsAsync(ct);
        return Result.Ok(result);
    }
}
