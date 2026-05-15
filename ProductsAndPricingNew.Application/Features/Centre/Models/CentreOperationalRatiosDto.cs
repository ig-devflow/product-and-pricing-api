namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreOperationalRatiosDto(
    decimal? Guarantees,
    decimal? IndividualsRatio,
    decimal? StaffingRatio,
    decimal? EmptyBeds
);