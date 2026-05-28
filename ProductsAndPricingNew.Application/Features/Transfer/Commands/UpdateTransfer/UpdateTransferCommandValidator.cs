using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Features.Transfer.Validation;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.UpdateTransfer;

internal sealed class UpdateTransferCommandValidator : TransferCommandValidatorBase<UpdateTransferCommand>
{
    public UpdateTransferCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Transfer id is required.");

        RuleFor(x => x.Version)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Version is required.")
            .IsValidRowVersion()
            .WithMessage("Version must be a valid row version token.");
    }
}
