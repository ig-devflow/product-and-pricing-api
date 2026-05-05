using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionById;

internal sealed class GetDivisionByIdQueryHandler : IRequestHandler<GetDivisionByIdQuery, Result<DivisionDetailsDto>>
{
    private readonly IDivisionQuery _divisionQuery;

    public GetDivisionByIdQueryHandler(IDivisionQuery divisionQuery)
    {
        _divisionQuery = divisionQuery;
    }

    public async Task<Result<DivisionDetailsDto>> Handle(GetDivisionByIdQuery request, CancellationToken ct)
    {
        DivisionDetailsDto? result = await _divisionQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Division with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}