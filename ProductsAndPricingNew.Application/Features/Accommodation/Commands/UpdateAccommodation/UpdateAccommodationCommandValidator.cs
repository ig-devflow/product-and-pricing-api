using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.Accommodation.Validation;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Commands.UpdateAccommodation;

internal sealed class UpdateAccommodationCommandValidator : AccommodationCommandValidatorBase<UpdateAccommodationCommand>
{
    public UpdateAccommodationCommandValidator(IReferenceDataValidationQuery referenceData) : base(referenceData)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Accommodation id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}