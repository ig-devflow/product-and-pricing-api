namespace ProductsAndPricingNew.Application.Features.Course.Models;

public sealed record CourseListItemDto(
    int Id,
    string DivisionName,
    string Name,
    bool IsActive,
    int CourseLanguageId,
    int CourseIntensityId,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
