using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

internal sealed class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, Result<DivisionDetailsDto>>
{
    private readonly IDivisionQueries _divisionQueries;

    public GetDivisionByIdQueryHandler(IDivisionQueries divisionQueries)
    {
        _divisionQueries = divisionQueries;
    }

    public async Task<Result<DivisionDetailsDto>> Handle(GetDivisionByIdQuery request, CancellationToken ct)
    {
        DivisionDetailsDto? result = await _divisionQueries.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail($"Division with id {request.Id} was not found");

        return Result.Ok(result);
    }
}