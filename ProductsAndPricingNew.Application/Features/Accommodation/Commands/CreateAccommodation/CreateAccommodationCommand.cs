using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.CreateAccommodation;

public sealed record CreateAccommodationCommand(
    string Name,
    int AccommodationTypeId,
    bool IsActive,
    int MinimumStayInWeeks,
    int? AgeFrom,
    int? AgeTo,
    bool IsCommitted,
    bool IsNonCommitted
) : ICommand<Result<int>>, IAccommodationCommandPayload;