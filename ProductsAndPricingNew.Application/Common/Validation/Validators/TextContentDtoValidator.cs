using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Abstractions;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Application.Common.Validation.Validators;

internal sealed class TextContentDtoValidator : AbstractValidator<TextContentDto>
{
    public TextContentDtoValidator(IReferenceDataValidationQuery referenceData, ContentTemplateScope scope)
    {
        RuleFor(x => x.ContentTemplateId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("ContentTemplateId is required.")
            .MustAsync((contentTemplateId, ct) => ContentTemplateExistsAsync(referenceData, contentTemplateId, scope, ct))
            .WithMessage($"ContentTemplateId must reference an active {scope} content template.");

        RuleFor(x => x.AudienceId)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0)
            .WithMessage("AudienceId must be greater than 0.")
            .MustAsync((audienceId, ct) => AudienceExistsAsync(referenceData, audienceId, ct))
            .WithMessage("AudienceId must reference an active audience.")
            .When(x => x.AudienceId.HasValue);

        RuleFor(x => x.Format)
            .IsInEnum();

        RuleFor(x => x.Content)
            .MaximumLength(FormattedText.Rules.ContentMaxLength)
            .WithMessage($"Content must not exceed {FormattedText.Rules.ContentMaxLength} characters.");

        RuleFor(x => x)
            .Must(HaveConsistentContentAndFormat)
            .WithMessage("Content format is inconsistent with content.");
    }

    private static bool HaveConsistentContentAndFormat(TextContentDto text)
    {
        bool hasContent = !string.IsNullOrWhiteSpace(text.Content);

        return hasContent
            ? text.Format != ContentFormat.None
            : text.Format == ContentFormat.None;
    }

    private static async Task<bool> ContentTemplateExistsAsync(
        IReferenceDataValidationQuery referenceData,
        int contentTemplateId,
        ContentTemplateScope scope,
        CancellationToken ct)
    {
        if (contentTemplateId <= 0)
            return true;

        IReadOnlySet<int> activeContentTemplateIds = await referenceData.GetActiveContentTemplateIdsAsync(new[] { contentTemplateId }, scope, ct);
        return activeContentTemplateIds.Contains(contentTemplateId);
    }

    private static async Task<bool> AudienceExistsAsync(IReferenceDataValidationQuery referenceData, int? audienceId, CancellationToken ct)
    {
        if (audienceId is null or <= 0)
            return true;

        IReadOnlySet<int> activeAudienceIds = await referenceData.GetActiveAudienceIdsAsync(new[] { audienceId.Value }, ct);
        return activeAudienceIds.Contains(audienceId.Value);
    }
}
