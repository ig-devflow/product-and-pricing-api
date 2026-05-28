using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Transfer.Validation;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.CreateTransfer;

internal sealed class CreateTransferCommandValidator : TransferCommandValidatorBase<CreateTransferCommand>
{
    public CreateTransferCommandValidator(IReferenceDataValidationQuery referenceDataValidation) : base(referenceDataValidation)
    {
        RuleFor(x => x.DivisionId)
            .GreaterThan(0)
            .WithMessage("DivisionId is required.");
    }
}
