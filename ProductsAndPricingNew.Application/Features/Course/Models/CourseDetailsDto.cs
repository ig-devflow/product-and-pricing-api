namespace ProductsAndPricingNew.Application.Features.Course.Models;

public sealed record CourseDetailsDto(
    int Id,
    int DivisionId,
    int UnitTypeId,
    int CourseLanguageId,
    int CourseIntensityId,
    string Name,
    bool IsActive,
    int? AgeFrom,
    int? AgeTo,
    int? MinimumWeeks,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
