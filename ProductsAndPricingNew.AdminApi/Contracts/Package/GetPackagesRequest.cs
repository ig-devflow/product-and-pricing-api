namespace ProductsAndPricingNew.AdminApi.Contracts.Package;

public sealed record GetPackagesRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);
