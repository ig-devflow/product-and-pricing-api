using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.UpdateCourse;

public sealed record UpdateCourseCommand(
    int Id,
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
    DateOnly? ClosurePolicy,
    string Version
) : ICommand<Result<Unit>>, ICourseCommandPayload;