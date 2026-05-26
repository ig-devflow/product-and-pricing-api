using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.UpdateAccommodation;

public sealed record UpdateAccommodationCommand(
    int Id,
    string Name,
    int AccommodationTypeId,
    bool IsActive,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    bool IsCommitted,
    bool IsNonCommitted,
    string Version
) : ICommand<Result<Unit>>, IAccommodationCommandPayload;