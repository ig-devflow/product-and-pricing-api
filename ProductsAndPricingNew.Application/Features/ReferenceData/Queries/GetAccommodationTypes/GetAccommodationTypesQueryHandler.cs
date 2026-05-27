using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationTypes;

internal sealed class GetAccommodationTypesQueryHandler : IRequestHandler<GetAccommodationTypesQuery, Result<IReadOnlyCollection<AccommodationTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAccommodationTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AccommodationTypeReferenceDto>>> Handle(GetAccommodationTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccommodationTypeReferenceDto> result = await _referenceDataQuery.GetAccommodationTypesAsync(ct);
        return Result.Ok(result);
    }
}