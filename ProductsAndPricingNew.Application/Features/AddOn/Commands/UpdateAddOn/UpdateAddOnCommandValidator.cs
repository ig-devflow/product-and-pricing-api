using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.AddOn.Validation;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.UpdateAddOn;

internal sealed class UpdateAddOnCommandValidator : AddOnCommandValidatorBase<UpdateAddOnCommand>
{
    public UpdateAddOnCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Add-on id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}
