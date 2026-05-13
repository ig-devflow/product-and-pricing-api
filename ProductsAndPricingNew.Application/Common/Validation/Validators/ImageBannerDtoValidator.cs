using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Validation.Limits;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Application.Common.Validation.Validators;

internal sealed class ImageBannerDtoValidator : AbstractValidator<ImageBannerDto>
{
    public ImageBannerDtoValidator(string displayName = "Image", int maxBytes = ValidationLimitDefaults.MaxFileBytes)
    {
        RuleFor(x => x.Data)
            .Must(data => data is null || data.Length <= maxBytes)
            .WithMessage($"{displayName} must not exceed {ToMegabytes(maxBytes)} MB.");

        When(x => x.Data is { Length: > 0 }, () =>
        {
            RuleFor(x => x.ContentType)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Content type is required.")
                .MaximumLength(ImageFile.Rules.ContentTypeMaxLength)
                .WithMessage($"Content type must not exceed {ImageFile.Rules.ContentTypeMaxLength} characters.")
                .Must(ImageFile.IsSupportedContentType)
                .WithMessage("Content type must be PNG/JPEG/WEBP/SVG.");

            RuleFor(x => x.FileName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("FileName is required.")
                .MaximumLength(ImageFile.Rules.FileNameMaxLength)
                .WithMessage($"FileName must not exceed {ImageFile.Rules.FileNameMaxLength} characters.");
        });
    }

    private static int ToMegabytes(int bytes) => bytes / 1024 / 1024;
}
