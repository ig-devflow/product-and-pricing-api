using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationRoomTypes;

internal sealed class GetAccommodationRoomTypesQueryHandler : IRequestHandler<GetAccommodationRoomTypesQuery, Result<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAccommodationRoomTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>>> Handle(GetAccommodationRoomTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccommodationRoomTypeReferenceDto> result = await _referenceDataQuery.GetAccommodationRoomTypesAsync(ct);
        return Result.Ok(result);
    }
}