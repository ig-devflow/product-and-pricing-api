using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationBoardTypes;

public sealed record GetAccommodationBoardTypesQuery() : IRequest<Result<IReadOnlyCollection<AccommodationBoardTypeReferenceDto>>>;