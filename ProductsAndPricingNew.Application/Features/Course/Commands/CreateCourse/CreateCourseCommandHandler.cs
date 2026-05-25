using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.Domain.UnitOfMeasure;
using CourseEntity = ProductsAndPricingNew.Domain.Entities.Products.Course;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.CreateCourse;

internal sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<int>>
{
    private readonly ICourseQuery _courseQuery;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitTypeProvider _unitTypeProvider;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(
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

    public async Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken ct)
    {
        string name = request.Name.AsRequiredText(nameof(request.Name));

        bool isNameTaken = await _courseQuery.ExistsByNameAsync(name, ct: ct);
        if (isNameTaken)
            return Result.Fail(new ConflictError($"Course name: '{name}' already exists"));

        UnitType unitType = _unitTypeProvider.Get(request.UnitTypeId);

        CourseEntity course = new CourseEntity.Builder(request.DivisionId, name, request.CourseLanguageId, request.CourseIntensityId, unitType)
            .SetIsActive(request.IsActive)
            .WithAgeRange(request.AgeFrom, request.AgeTo)
            .WithMinimumWeeks(request.MinimumWeeks)
            .WithCategories(request.AccountCategoryId, request.ProductCategoryId)
            .WithFinanceCodes(request.GeneralLedgerCode, request.CostCentreCode)
            .WithClosurePolicy(request.ClosurePolicy)
            .Build();

        await _courseRepository.AddAsync(course, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Ok(course.Id);
    }
}