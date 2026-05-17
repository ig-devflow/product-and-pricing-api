namespace ProductsAndPricingNew.AdminApi.Contracts.School;

public sealed record GetSchoolsRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null,
    int? CentreId = null
);