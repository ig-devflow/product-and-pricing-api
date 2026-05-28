using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Validation;

namespace ProductsAndPricingNew.Application.Features.Package.Commands.CreatePackage;

internal sealed class CreatePackageCommandValidator : PackageCommandValidatorBase<CreatePackageCommand>
{
    public CreatePackageCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.DivisionId)
            .GreaterThan(0)
            .WithMessage("DivisionId is required.");
    }
}
