using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Transfer.Models;

namespace ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransferById;

public sealed record GetTransferByIdQuery(int Id) : IRequest<Result<TransferDetailsDto>>;
