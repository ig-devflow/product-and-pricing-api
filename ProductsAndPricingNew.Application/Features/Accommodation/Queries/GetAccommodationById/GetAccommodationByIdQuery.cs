using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Accommodation.Models;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodationById;

public sealed record GetAccommodationByIdQuery(int Id) : IRequest<Result<AccommodationDetailsDto>>;