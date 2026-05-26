using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCentreContactTypes;

internal sealed class GetCentreContactTypesQueryHandler : IRequestHandler<GetCentreContactTypesQuery, Result<IReadOnlyCollection<CentreContactTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetCentreContactTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<CentreContactTypeReferenceDto>>> Handle(GetCentreContactTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<CentreContactTypeReferenceDto> result = await _referenceDataQuery.GetCentreContactTypesAsync(ct);
        return Result.Ok(result);
    }
}