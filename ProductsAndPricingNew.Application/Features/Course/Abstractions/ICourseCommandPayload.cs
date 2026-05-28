namespace ProductsAndPricingNew.Application.Features.Course.Abstractions;

internal interface ICourseCommandPayload
{
    int UnitTypeId { get; }
    int CourseLanguageId { get; }
    int CourseIntensityId { get; }
    string Name { get; }
    bool IsActive { get; }
    int AccountCategoryId { get; }
    int ProductCategoryId { get; }
    int? AgeFrom { get; }
    int? AgeTo { get; }
    int? MinimumWeeks { get; }
    string? GeneralLedgerCode { get; }
    string? CostCentreCode { get; }
    DateOnly? ClosurePolicy { get; }
}
