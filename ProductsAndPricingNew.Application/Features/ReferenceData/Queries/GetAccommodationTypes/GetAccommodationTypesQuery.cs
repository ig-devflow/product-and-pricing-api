using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationTypes;

public sealed record GetAccommodationTypesQuery() : IRequest<Result<IReadOnlyCollection<AccommodationTypeReferenceDto>>>;