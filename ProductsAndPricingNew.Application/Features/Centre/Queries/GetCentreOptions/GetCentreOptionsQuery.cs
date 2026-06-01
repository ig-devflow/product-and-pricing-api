using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentreOptions;

public sealed record GetCentreOptionsQuery : IRequest<Result<IReadOnlyCollection<CentreOptionDto>>>;
