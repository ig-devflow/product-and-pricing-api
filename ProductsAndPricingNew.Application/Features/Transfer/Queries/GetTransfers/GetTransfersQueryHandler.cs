using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Application.Features.Transfer.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransfers;

internal sealed class GetTransfersQueryHandler : IRequestHandler<GetTransfersQuery, Result<PagedResult<TransferListItemDto>>>
{
    private readonly ITransferQuery _transferQuery;

    public GetTransfersQueryHandler(ITransferQuery transferQuery)
    {
        _transferQuery = transferQuery;
    }

    public async Task<Result<PagedResult<TransferListItemDto>>> Handle(GetTransfersQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<TransferListItemDto> result = await _transferQuery.GetListAsync(
            request.DivisionId,
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}
