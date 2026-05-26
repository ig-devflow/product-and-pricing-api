using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodationById;

internal sealed class GetAccommodationByIdQueryHandlers : IRequestHandler<GetAccommodationByIdQuery, Result<AccommodationDetailsDto>>
{
    private readonly IAccommodationQuery _accommodationQuery;

    public GetAccommodationByIdQueryHandlers(IAccommodationQuery accommodationQuery)
    {
        _accommodationQuery = accommodationQuery;
    }

    public async Task<Result<AccommodationDetailsDto>> Handle(GetAccommodationByIdQuery request, CancellationToken ct)
    {
        AccommodationDetailsDto? result = await _accommodationQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Accommodation with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}