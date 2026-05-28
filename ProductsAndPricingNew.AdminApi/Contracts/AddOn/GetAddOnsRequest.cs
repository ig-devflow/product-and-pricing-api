namespace ProductsAndPricingNew.AdminApi.Contracts.AddOn;

public sealed record GetAddOnsRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);
