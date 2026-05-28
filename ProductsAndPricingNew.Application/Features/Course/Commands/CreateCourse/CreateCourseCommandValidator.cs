using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Validation;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.CreateCourse;

internal sealed class CreateCourseCommandValidator : CourseCommandValidatorBase<CreateCourseCommand>
{
    public CreateCourseCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.DivisionId)
            .GreaterThan(0)
            .WithMessage("DivisionId is required.");
    }
}
