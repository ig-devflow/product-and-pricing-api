using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.Course.Validation;

namespace ProductsAndPricingNew.Application.Features.Course.Commands.UpdateCourse;

internal sealed class UpdateCourseCommandValidator : CourseCommandValidatorBase<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Course id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}
