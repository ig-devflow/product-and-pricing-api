using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationRoomTypes;

public sealed record GetAccommodationRoomTypesQuery() : IRequest<Result<IReadOnlyCollection<AccommodationRoomTypeReferenceDto>>>;