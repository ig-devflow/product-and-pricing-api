using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Abstractions;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationBoardTypes;

internal sealed class GetAccommodationBoardTypesQueryHandler : IRequestHandler<GetAccommodationBoardTypesQuery, Result<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>>>
{
    private readonly IReferenceDataQuery _referenceDataQuery;

    public GetAccommodationBoardTypesQueryHandler(IReferenceDataQuery referenceDataQuery)
    {
        _referenceDataQuery = referenceDataQuery;
    }

    public async Task<Result<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>>> Handle(GetAccommodationBoardTypesQuery request, CancellationToken ct)
    {
        IReadOnlyCollection<AccommodationBoardTypeReferenceDto> result = await _referenceDataQuery.GetAccommodationBoardTypesAsync(ct);
        return Result.Ok(result);
    }
}