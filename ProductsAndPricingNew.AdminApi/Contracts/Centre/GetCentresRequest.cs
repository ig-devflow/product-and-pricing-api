namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record GetCentresRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);