using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.School.Validation;

namespace ProductsAndPricingNew.Application.Features.School.Commands.UpdateSchool;

internal sealed class UpdateSchoolCommandValidator : SchoolCommandValidatorBase<UpdateSchoolCommand>
{
    public UpdateSchoolCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("School id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}