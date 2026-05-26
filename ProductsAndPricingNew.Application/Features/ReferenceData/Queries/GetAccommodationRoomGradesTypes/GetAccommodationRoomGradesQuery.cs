using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.ReferenceData.Models;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Queries.GetAccommodationRoomGradesTypes;

public sealed record GetAccommodationRoomGradesQuery() : IRequest<Result<IReadOnlyCollection<AccommodationRoomGradeReferenceDto>>>;