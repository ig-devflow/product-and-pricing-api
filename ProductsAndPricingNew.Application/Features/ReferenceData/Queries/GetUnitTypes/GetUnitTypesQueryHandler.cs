using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetUnitTypes;

internal sealed class GetUnitTypesQueryHandler : IRequestHandler<GetUnitTypesQuery, Result<IReadOnlyCollection<UnitTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetUnitTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<UnitTypeReferenceDto>>> Handle(GetUnitTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<UnitTypeReferenceDto> result = await _referenceDataQuery.GetUnitTypesAsync(ct);
        return Result.Ok(result);
    }
}
