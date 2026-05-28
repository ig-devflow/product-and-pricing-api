namespace ProductsAndPricingNew.AdminApi.Contracts.Course;

public sealed record UpdateCourseRequest(
    string Name,
    int UnitTypeId,
    int CourseLanguageId,
    int CourseIntensityId,
    bool IsActive,
    int AccountCategoryId,
    int ProductCategoryId,
    int? AgeFrom,
    int? AgeTo,
    int? MinimumWeeks,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version
);
