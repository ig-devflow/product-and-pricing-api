using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Application.Features.Transfer.Models;

namespace ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransferById;

internal sealed class GetTransferByIdQueryHandler : IRequestHandler<GetTransferByIdQuery, Result<TransferDetailsDto>>
{
    private readonly ITransferQuery _transferQuery;

    public GetTransferByIdQueryHandler(ITransferQuery transferQuery)
    {
        _transferQuery = transferQuery;
    }

    public async Task<Result<TransferDetailsDto>> Handle(GetTransferByIdQuery request, CancellationToken ct)
    {
        TransferDetailsDto? result = await _transferQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Transfer with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
