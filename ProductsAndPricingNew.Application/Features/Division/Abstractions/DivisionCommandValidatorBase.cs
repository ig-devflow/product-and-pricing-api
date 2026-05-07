using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.Division.Abstractions;

internal abstract class DivisionCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IDivisionCommandPayload
{
    private const int DivisionNameMaxLength = 100;
    private const int WebsiteUrlMaxLength = 255;
    private const int EmailMaxLength = 50;
    private const int PhoneMaxLength = 50;
    private const int TermsAndConditionsMaxLength = 4000;
    private const int GroupsPaymentTermsMaxLength = 4000;

    protected DivisionCommandValidatorBase(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Division name is required.")
            .MaximumLength(DivisionNameMaxLength)
            .WithMessage($"Division name must not exceed {DivisionNameMaxLength} characters.")
            .Must(name => !name.Contains("''", StringComparison.Ordinal))
            .WithMessage("Division name must not contain consecutive apostrophes.");

        RuleFor(x => x.WebsiteUrl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Website is required.")
            .MaximumLength(WebsiteUrlMaxLength)
            .WithMessage($"Website must not exceed {WebsiteUrlMaxLength} characters.")
            .IsValidHttpUrl();

        RuleFor(x => x.TermsAndConditions)
            .MaximumLength(TermsAndConditionsMaxLength)
            .WithMessage($"Terms and conditions must not exceed {TermsAndConditionsMaxLength} characters.");

        RuleFor(x => x.GroupsPaymentTerms)
            .MaximumLength(GroupsPaymentTermsMaxLength)
            .WithMessage($"Groups payment terms must not exceed {GroupsPaymentTermsMaxLength} characters.");

        RuleFor(x => x.HeadOfficeEmail)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(EmailMaxLength)
            .WithMessage($"Head office email must not exceed {EmailMaxLength} characters.")
            .IsValidEmail()
            .When(x => !string.IsNullOrWhiteSpace(x.HeadOfficeEmail));

        RuleFor(x => x.HeadOfficeTelephoneNo)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(PhoneMaxLength)
            .WithMessage($"Head office telephone must not exceed {PhoneMaxLength} characters.")
            .IsValidPhone()
            .When(x => !string.IsNullOrWhiteSpace(x.HeadOfficeTelephoneNo));

        RuleFor(x => x.ContactAddress!)
            .SetValidator(new AddressDtoValidator(referenceData))
            .When(x => x.ContactAddress is not null);

        RuleFor(x => x.AccreditationBanner!)
            .SetValidator(new ImageBannerDtoValidator("Accreditation banner"))
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

        HashSet<(int ContentTemplateId, int? AudienceId)> keys = new();

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