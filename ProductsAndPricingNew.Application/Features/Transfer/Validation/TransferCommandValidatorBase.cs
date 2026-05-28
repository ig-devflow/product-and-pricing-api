using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Transfer.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using TransferAggregate = ProductsAndPricingNew.Domain.Entities.Products.Transfer;

namespace ProductsAndPricingNew.Application.Features.Transfer.Validation;

internal abstract class TransferCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ITransferCommandPayload
{
    protected TransferCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Transfer name is required.")
            .MaximumLength(TransferAggregate.Rules.NameMaxLength)
            .WithMessage($"Transfer name must not exceed {TransferAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.")
            .MustAsync((unitTypeId, ct) => UnitTypeIsActiveAsync(referenceDataValidation, unitTypeId, ct))
            .WithMessage("UnitTypeId must reference an active unit type.");

        RuleFor(x => x.TransferTypeId)
            .GreaterThan(0)
            .WithMessage("TransferTypeId is required.");

        RuleFor(x => x.TransferPortId)
            .GreaterThan(0)
            .WithMessage("TransferPortId is required.");

        RuleFor(x => x.AccountCategoryId)
            .GreaterThan(0)
            .WithMessage("AccountCategoryId is required.");

        RuleFor(x => x.ProductCategoryId)
            .GreaterThan(0)
            .WithMessage("ProductCategoryId is required.");

        RuleFor(x => x)
            .Must(x => x.TimeFrom.HasValue == x.TimeTo.HasValue)
            .WithName("TimeFrom")
            .WithMessage("TimeFrom and TimeTo must both be set or both be null.")
            .When(x => x.TimeFrom.HasValue || x.TimeTo.HasValue);

        RuleFor(x => x)
            .Must(x => !x.TimeFrom.HasValue || !x.TimeTo.HasValue || x.TimeFrom <= x.TimeTo)
            .WithName("TimeFrom")
            .WithMessage("TimeFrom cannot be later than TimeTo.")
            .When(x => x.TimeFrom.HasValue && x.TimeTo.HasValue);

        RuleFor(x => x.GeneralLedgerCode)
            .MaximumLength(FinanceCodes.Rules.MaxLength)
            .WithMessage($"General ledger code must not exceed {FinanceCodes.Rules.MaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.GeneralLedgerCode));

        RuleFor(x => x.CostCentreCode)
            .MaximumLength(FinanceCodes.Rules.MaxLength)
            .WithMessage($"Cost centre code must not exceed {FinanceCodes.Rules.MaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.CostCentreCode));

        RuleFor(x => x.ClosurePolicy)
            .Must(OfferingsClosurePolicy.IsValid)
            .WithMessage("Closure policy date cannot be in the past.")
            .When(x => x.ClosurePolicy.HasValue);
    }

    private static async Task<bool> UnitTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int unitTypeId, CancellationToken ct)
    {
        if (unitTypeId <= 0)
            return false;

        IReadOnlySet<int> unitTypesIds = await referenceData.GetActiveUnitTypesIdsAsync(new[] { unitTypeId }, ct);
        return unitTypesIds.Contains(unitTypeId);
    }
}
