using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetTransferTypes;

public sealed record GetTransferTypesQuery() : IRequest<Result<IReadOnlyCollection<TransferTypeReferenceDto>>>;
