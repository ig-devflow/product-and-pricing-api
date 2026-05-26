namespace ProductsAndPricingNew.AdminApi.Contracts.Accommodation;

public sealed record GetAccommodationsRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);