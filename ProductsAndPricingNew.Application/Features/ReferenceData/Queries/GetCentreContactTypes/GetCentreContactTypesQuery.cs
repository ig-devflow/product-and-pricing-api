using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetCentreContactTypes;

public sealed record GetCentreContactTypesQuery() : IRequest<Result<IReadOnlyCollection<CentreContactTypeReferenceDto>>>;