using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.Package.Validation;

namespace ProductsAndPricingNew.Application.Features.Package.Commands.UpdatePackage;

internal sealed class UpdatePackageCommandValidator : PackageCommandValidatorBase<UpdatePackageCommand>
{
    public UpdatePackageCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Package id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}
