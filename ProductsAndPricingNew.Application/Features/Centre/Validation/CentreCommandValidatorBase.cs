using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.Centre.Abstractions;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using CentreAggregate = ProductsAndPricingNew.Domain.Entities.PricingRef.Centre;

namespace ProductsAndPricingNew.Application.Features.Centre.Validation;

internal abstract class CentreCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICentreCommandPayload
{
    protected CentreCommandValidatorBase(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Centre name is required.")
            .MaximumLength(CentreAggregate.Rules.NameMaxLength)
            .WithMessage($"Centre name must not exceed {CentreAggregate.Rules.NameMaxLength} characters.");

        RuleFor(x => x.Code)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Centre code is required.")
            .MaximumLength(CentreAggregate.Rules.CodeMaxLength)
            .WithMessage($"Centre code must not exceed {CentreAggregate.Rules.CodeMaxLength} characters.");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency is required.");

        RuleFor(x => x.PrintFormat)
            .Must(pf => Enum.IsDefined(pf) && pf != PrintFormat.None)
            .WithMessage("PrintFormat must be a valid value.");

        RuleFor(x => x.ContactInfo.GeneralEmail)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(EmailAddress.Rules.MaxLength)
            .WithMessage($"General email must not exceed {EmailAddress.Rules.MaxLength} characters.")
            .Must(EmailAddress.IsValid)
            .WithMessage("Invalid general email.");

        RuleFor(x => x.ContactInfo.AccommodationEmail)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(EmailAddress.Rules.MaxLength)
            .WithMessage($"Accommodation email must not exceed {EmailAddress.Rules.MaxLength} characters.")
            .Must(EmailAddress.IsValid)
            .WithMessage("Invalid accommodation email.");

        RuleFor(x => x.ContactInfo.Telephone)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(TelephoneNumber.Rules.MaxLength)
            .WithMessage($"Telephone must not exceed {TelephoneNumber.Rules.MaxLength} characters.")
            .Must(TelephoneNumber.IsValid)
            .WithMessage($"Telephone must include area/country code and contain {TelephoneNumber.Rules.MinDigits}-{TelephoneNumber.Rules.MaxDigits} digits.");

        RuleFor(x => x.ContactInfo.EmergencyTelephone)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(TelephoneNumber.Rules.MaxLength)
            .WithMessage($"Emergency telephone must not exceed {TelephoneNumber.Rules.MaxLength} characters.")
            .Must(TelephoneNumber.IsValid)
            .WithMessage($"Emergency telephone must include area/country code and contain {TelephoneNumber.Rules.MinDigits}-{TelephoneNumber.Rules.MaxDigits} digits.");

        RuleFor(x => x.ContactInfo.TransferEmergencyTelephone)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(TelephoneNumber.Rules.MaxLength)
            .WithMessage($"Transfer emergency telephone must not exceed {TelephoneNumber.Rules.MaxLength} characters.")
            .Must(TelephoneNumber.IsValid)
            .WithMessage($"Transfer emergency telephone must include area/country code and contain {TelephoneNumber.Rules.MinDigits}-{TelephoneNumber.Rules.MaxDigits} digits.");

        RuleFor(x => x.ContactInfo.BrandColor)
            .Must(HexColor.IsValid)
            .WithMessage("Brand color must be a valid hex color (e.g. #RRGGBB or #RGB).");

        RuleFor(x => x.ContactInfo.ContactAddress!)
            .SetValidator(new AddressDtoValidator(referenceData))
            .When(x => x.ContactInfo.ContactAddress is not null);

        RuleFor(x => x.ContactInfo.LogoImage!)
            .SetValidator(new ImageBannerDtoValidator("Logo", CentreAggregate.Rules.LogoMaxBytes))
            .When(x => x.ContactInfo.LogoImage is not null);

        RuleFor(x => x.LegalInfo.SchoolSponsorshipNumber)
            .MaximumLength(CentreAggregate.Rules.LegalTextMaxLength)
            .WithMessage($"School sponsorship number must not exceed {CentreAggregate.Rules.LegalTextMaxLength} characters.");

        RuleFor(x => x.LegalInfo.VatNumber)
            .MaximumLength(CentreAggregate.Rules.LegalTextMaxLength)
            .WithMessage($"VAT number must not exceed {CentreAggregate.Rules.LegalTextMaxLength} characters.");

        RuleFor(x => x.LegalInfo.RegistrationNumber)
            .MaximumLength(CentreAggregate.Rules.LegalTextMaxLength)
            .WithMessage($"Registration number must not exceed {CentreAggregate.Rules.LegalTextMaxLength} characters.");

        RuleFor(x => x.LegalInfo.VatExemptionNumber)
            .MaximumLength(CentreAggregate.Rules.LegalTextMaxLength)
            .WithMessage($"VAT exemption number must not exceed {CentreAggregate.Rules.LegalTextMaxLength} characters.");

        RuleFor(x => x.LegalInfo.ChequePayableTo)
            .MaximumLength(CentreAggregate.Rules.LegalTextMaxLength)
            .WithMessage($"Cheque payable to must not exceed {CentreAggregate.Rules.LegalTextMaxLength} characters.");

        RuleFor(x => x.BankDetails)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Bank details are required.")
            .SetValidator(new CentreBankDetailsDtoValidator(referenceData));

        RuleFor(x => x.Contacts)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Contacts collection is required.")
            .Must(NotContainDuplicateContactTypes)
            .WithMessage("Duplicate contact for the same contact type.");

        RuleForEach(x => x.Contacts)
            .SetValidator(new CentreContactDtoValidator());

        RuleFor(x => x.Texts)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage("Texts collection is required.")
            .Must(NotContainDuplicateTextKeys)
            .WithMessage("Duplicate text content for the same template and audience.");

        RuleForEach(x => x.Texts)
            .SetValidator(new TextContentDtoValidator(referenceData, ContentTemplateScope.Centre));
    }

    private static bool NotContainDuplicateContactTypes(IReadOnlyCollection<CentreContactDto>? contacts)
    {
        if (contacts is null)
            return true;

        var types = new HashSet<CentreContactType>();
        foreach (CentreContactDto contact in contacts)
        {
            if (!types.Add(contact.ContactType))
                return false;
        }

        return true;
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
