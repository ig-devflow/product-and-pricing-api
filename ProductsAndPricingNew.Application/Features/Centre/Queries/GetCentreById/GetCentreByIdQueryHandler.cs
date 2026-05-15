using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentreById;

internal sealed class GetCentreByIdQueryHandler : IRequestHandler<GetCentreByIdQuery, Result<CentreDetailsDto>>
{
    private readonly ICentreQuery _centreQuery;

    public GetCentreByIdQueryHandler(ICentreQuery centreQuery)
    {
        _centreQuery = centreQuery;
    }

    public async Task<Result<CentreDetailsDto>> Handle(GetCentreByIdQuery request, CancellationToken ct)
    {
        CentreDetailsDto? result = await _centreQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Centre with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}