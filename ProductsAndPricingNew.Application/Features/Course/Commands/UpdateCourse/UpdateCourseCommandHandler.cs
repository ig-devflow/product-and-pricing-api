using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using CourseEntity = ProductsAndPricingNew.Domain.Entities.Products.Course;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.UpdateCourse;

internal sealed class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result<Unit>>
{
    private readonly ICourseQuery _courseQuery;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseCommandHandler(
        ICourseQuery courseQuery,
        ICourseRepository courseRepository,
        IUnitTypeProvider unitTypeProvider,
        IUnitOfWork unitOfWork)
    {
        _courseQuery = courseQuery;
        _courseRepository = courseRepository;
        _unitTypeProvider = unitTypeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(UpdateCourseCommand request, CancellationToken ct)
    {
        CourseEntity? course = await _courseRepository.GetByIdAsync(request.Id, ct);
        if (course is null)
            return Result.Fail(new NotFoundError($"Course with id {request.Id} was not found"));

        if (!course.HasVersion(request.Version))
            return Result.Fail(new ConflictError("Course was modified by another user. Reload it and try again."));

        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _courseQuery.ExistsByNameAsync(name, request.Id, ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Course name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        course.Rename(name);
        course.SetIsActive(request.IsActive);
        course.WithLanguage(request.CourseLanguageId);
        course.WithUnitType(unitType);
        course.WithIntensity(request.CourseIntensityId);
        course.WithAgeRange(request.AgeFrom, request.AgeTo);
        course.WithMinimumWeeks(request.MinimumWeeks);
        course.WithCategories(request.AccountCategoryId, request.ProductCategoryId);
        course.WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode);
        course.WithClosurePolicy(request.ClosurePolicy);

        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok();
    }
}