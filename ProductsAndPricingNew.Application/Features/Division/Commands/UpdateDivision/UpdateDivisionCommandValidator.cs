using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Division.Validation;

namespace ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;

internal sealed class UpdateDivisionCommandValidator : DivisionCommandValidatorBase<UpdateDivisionCommand>
{
    public UpdateDivisionCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Division id is required.");
    }
}