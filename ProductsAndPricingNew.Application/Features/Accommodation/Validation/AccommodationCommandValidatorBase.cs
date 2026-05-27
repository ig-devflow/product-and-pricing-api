using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Features.Accommodation.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using AccommodationAggregate = ProductsAndPricingNew.Domain.Entities.Products.Accommodation;

namespace ProductsAndPricingNew.Application.Features.Accommodation.Validation;

internal abstract class AccommodationCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IAccommodationCommandPayload
{
    protected AccommodationCommandValidatorBase(IReferenceDataValidationQuery referenceDataValidation)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Accommodation name is required.")
            .MaximumLength(AccommodationAggregate.Rules.NameMaxLength)
            .WithMessage($"Accommodation name must not exceed {AccommodationAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.AccommodationTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("AccommodationTypeId is required.")
            .MustAsync((accommodationTypeId, ct) => AccommodationTypeIsActiveAsync(referenceDataValidation, accommodationTypeId, ct))
            .WithMessage("AccommodationTypeId must reference an active accommodation type.");

        RuleFor(x => x.MinimumStayInWeeks)
            .Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(0)
            .WithMessage("MinimumStayInWeeks cannot be less than 0.");

        RuleFor(x => x.AgeFrom)
            .InclusiveBetween(AgeRange.Rules.MinAge, AgeRange.Rules.MaxAge)
            .WithMessage($"Age from must be between {AgeRange.Rules.MinAge} and {AgeRange.Rules.MaxAge}.")
            .When(x => x.AgeFrom.HasValue);

        RuleFor(x => x.AgeTo)
            .InclusiveBetween(AgeRange.Rules.MinAge, AgeRange.Rules.MaxAge)
            .WithMessage($"Age to must be between {AgeRange.Rules.MinAge} and {AgeRange.Rules.MaxAge}.")
            .When(x => x.AgeTo.HasValue);

        RuleFor(x => x)
            .Must(x => !x.AgeFrom.HasValue || !x.AgeTo.HasValue || x.AgeFrom <= x.AgeTo)
            .WithMessage("Age from must be less than or equal to age to.")
            .When(x => x.AgeFrom.HasValue && x.AgeTo.HasValue);
    }

    private static async Task<bool> AccommodationTypeIsActiveAsync(IReferenceDataValidationQuery referenceDataValidation, int accommodationTypeId, CancellationToken ct)
    {
        if (accommodationTypeId <= 0)
            return false;

        IReadOnlySet<int> accommodationTypeIds = await referenceDataValidation.GetActiveAccommodationTypesIdsAsync(new[] { accommodationTypeId }, ct);
        return accommodationTypeIds.Contains(accommodationTypeId);
    }
}