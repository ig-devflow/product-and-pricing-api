using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Extensions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.Division.Abstractions;
using DivisionAggregate = ProductsAndPricingNew.Domain.Entities.PricingRef.Division;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.Division.Validation;

internal abstract class DivisionCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IDivisionCommandPayload
{
    protected DivisionCommandValidatorBase(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Division name is required.")
            .MaximumLength(DivisionAggregate.Rules.NameMaxLength)
            .WithMessage($"Division name must not exceed {DivisionAggregate.Rules.NameMaxLength} characters.")
            .Must(name => !name.Contains("''", StringComparison.Ordinal))
            .WithMessage("Division name must not contain consecutive apostrophes.");

        RuleFor(x => x.WebsiteUrl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Website is required.")
            .MaximumLength(DivisionAggregate.Rules.WebsiteUrlMaxLength)
            .WithMessage($"Website must not exceed {DivisionAggregate.Rules.WebsiteUrlMaxLength} characters.")
            .IsValidHttpUrl();

        RuleFor(x => x.TermsAndConditions)
            .MaximumLength(DivisionAggregate.Rules.TermsAndConditionsMaxLength)
            .WithMessage($"Terms and conditions must not exceed {DivisionAggregate.Rules.TermsAndConditionsMaxLength} characters.");

        RuleFor(x => x.GroupsPaymentTerms)
            .MaximumLength(DivisionAggregate.Rules.GroupsPaymentTermsMaxLength)
            .WithMessage($"Groups payment terms must not exceed {DivisionAggregate.Rules.GroupsPaymentTermsMaxLength} characters.");

        RuleFor(x => x.HeadOfficeEmail)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(DivisionAggregate.Rules.HeadOfficeEmailMaxLength)
            .WithMessage($"Head office email must not exceed {DivisionAggregate.Rules.HeadOfficeEmailMaxLength} characters.")
            .IsValidEmail()
            .When(x => !string.IsNullOrWhiteSpace(x.HeadOfficeEmail));

        RuleFor(x => x.HeadOfficeTelephoneNo)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(DivisionAggregate.Rules.HeadOfficeTelephoneNoMaxLength)
            .WithMessage($"Head office telephone must not exceed {DivisionAggregate.Rules.HeadOfficeTelephoneNoMaxLength} characters.")
            .IsValidPhone()
            .When(x => !string.IsNullOrWhiteSpace(x.HeadOfficeTelephoneNo));

        RuleFor(x => x.ContactAddress!)
            .SetValidator(new AddressDtoValidator(referenceData))
            .When(x => x.ContactAddress is not null);

        RuleFor(x => x.AccreditationBanner!)
            .SetValidator(new ImageBannerDtoValidator("Accreditation banner", DivisionAggregate.Rules.AccreditationBannerMaxBytes))
            .When(x => x.AccreditationBanner is not null);

        RuleFor(x => x.Texts)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Texts collection is required.")
            .Must(NotContainDuplicateTextKeys)
            .WithMessage("Duplicate text content for the same template and audience.");

        RuleForEach(x => x.Texts)
            .SetValidator(new TextContentDtoValidator(referenceData, ContentTemplateScope.Division));
    }

    private static bool NotContainDuplicateTextKeys(IReadOnlyCollection<TextContentDto>? texts)
    {
        if (texts is null)
            return true;

        var keys = new HashSet<(int ContentTemplateId, int? AudienceId)>();

        foreach (TextContentDto text in texts)
        {
            if (text.ContentTemplateId <= 0)
                continue;

            int? audienceId = text.AudienceId > 0
                ? text.AudienceId
                : null;

            if (!keys.Add((text.ContentTemplateId, audienceId)))
                return false;
        }

        return true;
    }
}
