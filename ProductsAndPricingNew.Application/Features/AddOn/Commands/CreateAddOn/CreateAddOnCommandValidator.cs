using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Validation;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.CreateAddOn;

internal sealed class CreateAddOnCommandValidator : AddOnCommandValidatorBase<CreateAddOnCommand>
{
    public CreateAddOnCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.DivisionId)
            .GreaterThan(0)
            .WithMessage("DivisionId is required.");
    }
}
