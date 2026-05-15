using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Centre.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Queries.GetCentreById;

public sealed record GetCentreByIdQuery(int Id) : IRequest<Result<CentreDetailsDto>>;