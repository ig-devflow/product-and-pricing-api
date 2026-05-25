using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetUnitTypes;

public sealed record GetUnitTypesQuery() : IRequest<Result<IReadOnlyCollection<UnitTypeReferenceDto>>>;
