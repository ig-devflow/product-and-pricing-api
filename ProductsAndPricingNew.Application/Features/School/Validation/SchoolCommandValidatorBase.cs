using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.School.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using SchoolAggregate = ProductsAndPricingNew.Domain.Entities.PricingRef.School;

namespace ProductsAndPricingNew.Application.Features.School.Validation;

internal abstract class SchoolCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ISchoolCommandPayload
{
    protected SchoolCommandValidatorBase(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("School name is required.")
            .MaximumLength(SchoolAggregate.Rules.NameMaxLength)
            .WithMessage($"School name must not exceed {SchoolAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.LegacyCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Legacy code is required.")
            .MaximumLength(SchoolAggregate.Rules.LegacyCodeMaxLength)
            .WithMessage($"Legacy code must not exceed {SchoolAggregate.Rules.LegacyCodeMaxLength} characters.");

        RuleFor(x => x.MinimumStayInWeeks)
            .GreaterThan(0)
            .WithMessage("Minimum stay in weeks must be greater than zero.");

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

        RuleFor(x => x.Telephone)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(TelephoneNumber.Rules.MaxLength)
            .WithMessage($"Telephone must not exceed {TelephoneNumber.Rules.MaxLength} characters.")
            .Must(TelephoneNumber.IsValid)
            .WithMessage($"Telephone must include area/country code and contain {TelephoneNumber.Rules.MinDigits}-{TelephoneNumber.Rules.MaxDigits} digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.Telephone));

        RuleFor(x => x.EmergencyTelephone)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(TelephoneNumber.Rules.MaxLength)
            .WithMessage($"Emergency telephone must not exceed {TelephoneNumber.Rules.MaxLength} characters.")
            .Must(TelephoneNumber.IsValid)
            .WithMessage($"Emergency telephone must include area/country code and contain {TelephoneNumber.Rules.MinDigits}-{TelephoneNumber.Rules.MaxDigits} digits.")
            .When(x => !string.IsNullOrWhiteSpace(x.EmergencyTelephone));

        RuleFor(x => x.ContactAddress!)
            .SetValidator(new AddressDtoValidator(referenceData))
            .When(x => x.ContactAddress is not null);

        RuleFor(x => x.FinanceCode)
            .MaximumLength(FinanceCode.Rules.MaxLength)
            .WithMessage($"Finance code must not exceed {FinanceCode.Rules.MaxLength} characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FinanceCode));
    }
}