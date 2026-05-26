using FluentValidation;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Application.Common.Validation.Validators;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Application.Features.Centre.Validation;

internal sealed class CentreContactDtoValidator : AbstractValidator<CentreContactDto>
{
    public CentreContactDtoValidator(IReferenceDataValidationQuery referenceData)
    {
        RuleFor(x => x.ContactTypeId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("Contact type id must be specified.")
            .MustAsync((contactId, ct) => ContactTypeIsActiveAsync(referenceData, contactId, ct))
            .WithMessage("Contact type must reference an active contact type."); ;

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Contact name is required.")
            .MaximumLength(CentreContact.Rules.NameMaxLength)
            .WithMessage($"Contact name must not exceed {CentreContact.Rules.NameMaxLength} characters.");

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .MaximumLength(EmailAddress.Rules.MaxLength)
            .WithMessage($"Contact email must not exceed {EmailAddress.Rules.MaxLength} characters.")
            .Must(EmailAddress.IsValid)
            .WithMessage("Invalid contact email.");

        RuleFor(x => x.SignatureImage)
            .SetValidator(new ImageBannerDtoValidator("Signature image", CentreContact.Rules.SignatureMaxBytes));
    }

    private static async Task<bool> ContactTypeIsActiveAsync(IReferenceDataValidationQuery referenceData, int? contactTypeId, CancellationToken ct)
    {
        if (contactTypeId is null or <= 0)
            return true;

        IReadOnlySet<int> activeContactTypeIds = await referenceData.GetActiveContactTypeIdsAsync(new[] { contactTypeId.Value }, ct);
        return activeContactTypeIds.Contains(contactTypeId.Value);
    }
}