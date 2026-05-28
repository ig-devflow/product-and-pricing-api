using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Models;

namespace ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransfers;

public sealed record GetTransfersQuery(
    int DivisionId,
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<TransferListItemDto>>>;
