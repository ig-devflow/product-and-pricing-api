using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using AddOnAggregate = ProductsAndPricingNew.Domain.Entities.Products.AddOn;

namespace ProductsAndPricingNew.Application.Features.AddOn.Validation;

internal abstract class AddOnCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IAddOnCommandPayload
{
    protected AddOnCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Add-on name is required.")
            .MaximumLength(AddOnAggregate.Rules.NameMaxLength)
            .WithMessage($"Add-on name must not exceed {AddOnAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.")
            .MustAsync((unitTypeId, ct) => UnitTypeIsActiveAsync(referenceDataValidation, unitTypeId, ct))
            .WithMessage("UnitTypeId must reference an active unit type.");

        RuleFor(x => x.AddOnType)
            .IsInEnum()
            .WithMessage("AddOnType must be a valid value.");

        RuleFor(x => x.AccountCategoryId)
            .GreaterThan(0)
            .WithMessage("AccountCategoryId is required.");

        RuleFor(x => x.ProductCategoryId)
            .GreaterThan(0)
            .WithMessage("ProductCategoryId is required.");

        RuleFor(x => x.AgeFrom)
            .InclusiveBetween(AgeRange.Rules.MinAge, AgeRange.Rules.MaxAge)
            .WithMessage($"AgeFrom must be between {AgeRange.Rules.MinAge} and {AgeRange.Rules.MaxAge}.")
            .When(x => x.AgeFrom.HasValue);

        RuleFor(x => x.AgeTo)
            .InclusiveBetween(AgeRange.Rules.MinAge, AgeRange.Rules.MaxAge)
            .WithMessage($"AgeTo must be between {AgeRange.Rules.MinAge} and {AgeRange.Rules.MaxAge}.")
            .When(x => x.AgeTo.HasValue);

        RuleFor(x => x)
            .Must(x => !x.AgeFrom.HasValue || !x.AgeTo.HasValue || x.AgeFrom <= x.AgeTo)
            .WithName("AgeFrom")
            .WithMessage("AgeFrom must be less than or equal to AgeTo.")
            .When(x => x.AgeFrom.HasValue && x.AgeTo.HasValue);

        RuleFor(x => x.OneToOneLessonsPerWeek)
            .GreaterThanOrEqualTo(1)
            .WithMessage("OneToOneLessonsPerWeek must be at least 1 for OneToOneCourse add-on type.")
            .When(x => x.AddOnType == AddOnType.OneToOneCourse);

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
