using FluentValidation;
using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Common.Validation.Validators;

internal sealed class ImageBannerDtoValidator : AbstractValidator<ImageBannerDto>
{
    private const int BannerContentTypeMaxLength = 100;
    private const int BannerFileNameMaxLength = 255;
    private static readonly HashSet<string> AllowedImageContentTypes = new(StringComparer.OrdinalIgnoreCase) { "image/png", "image/jpeg", "image/jpg", "image/webp", "image/svg+xml" };

    public ImageBannerDtoValidator(string displayName = "Image", int maxBytes = ValidationDefaults.MaxAccreditationBannerBytes)
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
                .MaximumLength(BannerContentTypeMaxLength)
                .WithMessage($"Content type must not exceed {BannerContentTypeMaxLength} characters.")
                .Must(contentType => !string.IsNullOrWhiteSpace(contentType) && AllowedImageContentTypes.Contains(contentType.Trim()))
                .WithMessage("Content type must be PNG/JPEG/WEBP/SVG.");

            RuleFor(x => x.FileName)
                .MaximumLength(BannerFileNameMaxLength)
                .WithMessage($"FileName must not exceed {BannerFileNameMaxLength} characters.");
        });
    }

    private static int ToMegabytes(int bytes)
    {
        return bytes / 1024 / 1024;
    }
}