namespace ProductsAndPricingNew.AdminApi.Contracts.Course;

public sealed record GetCoursesRequest(
    string? Search = null,
    bool? IsActive = null,
    int? Page = null,
    int? PageSize = null
);
