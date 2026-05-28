using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Course.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using CourseAggregate = ProductsAndPricingNew.Domain.Entities.Products.Course;

namespace ProductsAndPricingNew.Application.Features.Course.Validation;

internal abstract class CourseCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICourseCommandPayload
{
    protected CourseCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Course name is required.")
            .MaximumLength(CourseAggregate.Rules.NameMaxLength)
            .WithMessage($"Course name must not exceed {CourseAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.UnitTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("UnitTypeId is required.")
            .MustAsync((unitTypeId, ct) => UnitTypeIsActiveAsync(referenceDataValidation, unitTypeId, ct))
            .WithMessage("UnitTypeId must reference an active unit type.");

        RuleFor(x => x.CourseLanguageId)
            .GreaterThan(0)
            .WithMessage("CourseLanguageId is required.");

        RuleFor(x => x.CourseIntensityId)
            .GreaterThan(0)
            .WithMessage("CourseIntensityId is required.");

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
    }

    private static async Task<bool> UnitTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int unitTypeId, CancellationToken ct)
    {
        if (unitTypeId <= 0)
            return false;

        IReadOnlySet<int> unitTypesIds = await referenceData.GetActiveUnitTypesIdsAsync(new[] { unitTypeId }, ct);
        return unitTypesIds.Contains(unitTypeId);
    }
}
