using FluentResults;
using MediatR;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.CreateCourse;

public sealed record CreateCourseCommand(
    int DivisionId,
    int UnitTypeId,
    int CourseLanguageId,
    int CourseIntensityId,
    string Name,
    bool IsActive,
    int ProductCategoryId,
    int AccountCategoryId,
    int? AgeFrom,
    int? AgeTo,
    int? MinimumWeeks,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
) : IRequest<Result<int>>;