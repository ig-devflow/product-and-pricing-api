using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Division.Models;

namespace ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisionOptions;

public sealed record GetDivisionOptionsQuery : IRequest<Result<IReadOnlyCollection<DivisionOptionDto>>>;
