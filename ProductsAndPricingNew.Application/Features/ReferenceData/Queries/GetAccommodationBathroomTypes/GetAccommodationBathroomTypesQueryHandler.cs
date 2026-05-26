using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationBathroomTypes;

internal sealed class GetAccommodationBathroomTypesQueryHandler : IRequestHandler<GetAccommodationBathroomTypesQuery, Result<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAccommodationBathroomTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>>> Handle(GetAccommodationBathroomTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccommodationBathroomTypeReferenceDto> result = await _referenceDataQuery.GetAccommodationBathroomTypesAsync(ct);
        return Result.Ok(result);
    }
}