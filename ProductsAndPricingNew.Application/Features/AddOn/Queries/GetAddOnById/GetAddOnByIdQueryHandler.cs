using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Models;

namespace ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOnById;

internal sealed class GetAddOnByIdQueryHandler : IRequestHandler<GetAddOnByIdQuery, Result<AddOnDetailsDto>>
{
    private readonly IAddOnQuery _addOnQuery;

    public GetAddOnByIdQueryHandler(IAddOnQuery addOnQuery)
    {
        _addOnQuery = addOnQuery;
    }

    public async Task<Result<AddOnDetailsDto>> Handle(GetAddOnByIdQuery request, CancellationToken ct)
    {
        AddOnDetailsDto? result = await _addOnQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Add-on with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
