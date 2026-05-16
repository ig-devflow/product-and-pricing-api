using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.Centre.Validation;

namespace ProductsAndPricingNew.Application.Features.Centre.Commands.UpdateCentre;

internal sealed class UpdateCentreCommandValidator : CentreCommandValidatorBase<UpdateCentreCommand>
{
    public UpdateCentreCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Centre id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}