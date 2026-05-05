using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Domain.Entities.Common;

public sealed record FormattedText
{
    public string? Content { get; private set; }
    public ContentFormat Format { get; private set; } = ContentFormat.None;

    private FormattedText()
    {
    }

    private FormattedText(string? content, ContentFormat format)
    {
        if (!Enum.IsDefined(format))
            throw new DomainException($"Unsupported content format '{format}'.");

        bool hasContent = !string.IsNullOrWhiteSpace(content);

        if (format == ContentFormat.None && hasContent)
            throw new DomainException("Content format must be provided when content is not empty.");

        if (format != ContentFormat.None && !hasContent)
            throw new DomainException("Content must be provided.");

        Content = hasContent ? content!.Trim() : null;
        Format = hasContent ? format : ContentFormat.None;
    }

    public static FormattedText Create(string? content, ContentFormat format)
        => new(content, format);

    public static FormattedText Empty { get; } = new(null, ContentFormat.None);
}