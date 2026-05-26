using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationRoomGradesTypes;

internal sealed class GetAccommodationRoomGradesQueryHandler : IRequestHandler<GetAccommodationRoomGradesQuery, Result<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAccommodationRoomGradesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>>> Handle(GetAccommodationRoomGradesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccommodationRoomGradeReferenceDto> result = await _referenceDataQuery.GetAccommodationRoomGradesAsync(ct);
        return Result.Ok(result);
    }
}