namespace ProductsAndPricingNew.AdminApi.Contracts.Division;

public sealed record GetDivisionsRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);