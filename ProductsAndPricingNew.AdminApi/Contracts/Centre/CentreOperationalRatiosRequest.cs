namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreOperationalRatiosRequest(
    decimal? Guarantees,
    decimal? IndividualsRatio,
    decimal? StaffingRatio,
    decimal? EmptyBeds
);