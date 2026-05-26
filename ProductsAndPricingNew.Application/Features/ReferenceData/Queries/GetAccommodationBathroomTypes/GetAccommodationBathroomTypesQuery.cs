using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationBathroomTypes;

public sealed record GetAccommodationBathroomTypesQuery() : IRequest<Result<IReadOnlyCollection<AccommodationBathroomTypeReferenceDto>>>;