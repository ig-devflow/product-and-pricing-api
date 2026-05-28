using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using PackageAggregate = ProductsAndPricingNew.Domain.Entities.Products.Package;

namespace ProductsAndPricingNew.Application.Features.Package.Validation;

internal abstract class PackageCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IPackageCommandPayload
{
    private const int DescriptionMaxLength = 4000;

    protected PackageCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Package name is required.")
            .MaximumLength(PackageAggregate.Rules.NameMaxLength)
            .WithMessage($"Package name must not exceed {PackageAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.")
            .MustAsync((unitTypeId, ct) => UnitTypeIsActiveAsync(referenceDataValidation, unitTypeId, ct))
            .WithMessage("UnitTypeId must reference an active unit type.");

        RuleFor(x => x.Description)
            .MaximumLength(DescriptionMaxLength)
            .WithMessage($"Description must not exceed {DescriptionMaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Description));

        RuleFor(x => x.Commission)
            .GreaterThan(0m)
            .WithMessage("Commission must be greater than 0.")
            .LessThanOrEqualTo(100m)
            .WithMessage("Commission must not exceed 100.");

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

        RuleFor(x => x.MinimumWeeks)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumWeeks must be 0 or greater.")
            .When(x => x.MinimumWeeks.HasValue);

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

        RuleFor(x => x.Items)
            .NotNull()
            .WithMessage("Items are required.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .GreaterThan(0)
                    .WithMessage("Package item ProductId must be greater than 0.");

                item.RuleFor(i => i.PriceBreakdown)
                    .GreaterThan(0m)
                    .WithMessage("Package item PriceBreakdown must be greater than 0.")
                    .LessThanOrEqualTo(100m)
                    .WithMessage("Package item PriceBreakdown must not exceed 100.");
            });
    }

    private static async Task<bool> UnitTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int unitTypeId, CancellationToken ct)
    {
        if (unitTypeId <= 0)
            return false;

        IReadOnlySet<int> unitTypesIds = await referenceData.GetActiveUnitTypesIdsAsync(new[] { unitTypeId }, ct);
        return unitTypesIds.Contains(unitTypeId);
    }
}
