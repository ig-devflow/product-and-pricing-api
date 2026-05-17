using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.School.Validation;

namespace ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;

internal sealed class CreateSchoolCommandValidator : SchoolCommandValidatorBase<CreateSchoolCommand>
{
    public CreateSchoolCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.CentreId)
            .GreaterThan(0)
            .WithMessage("Centre is required.");
    }
}