namespace ProductsAndPricingNew.AdminApi.Contracts.Transfer;

public sealed record GetTransfersRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);
