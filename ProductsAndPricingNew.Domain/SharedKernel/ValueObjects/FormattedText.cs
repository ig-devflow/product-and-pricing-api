using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public sealed record FormattedText : IEmptyValueObject
{
    public string? Content { get; private set; }
    public ContentFormat Format { get; private set; } = ContentFormat.None;

    public bool IsEmpty => Content is null && Format == ContentFormat.None;
    public static FormattedText Empty { get; } = new(null, ContentFormat.None);

    private FormattedText() { }

    private FormattedText(string? content, ContentFormat format)
    {
        if (!Enum.IsDefined(format))
            throw new DomainException($"Unsupported content format '{format}'.");

        bool hasContent = !string.IsNullOrWhiteSpace(content);

        if (!hasContent)
        {
            if (format != ContentFormat.None)
                throw new DomainException("Content must be provided.");

            Content = null;
            Format = ContentFormat.None;
            return;
        }

        if (format == ContentFormat.None)
            throw new DomainException("Content format must be provided when content is not empty.");

        Content = content.AsRequiredDomainText(nameof(Content), Rules.ContentMaxLength);
        Format = format;
    }

    public static FormattedText Create(string? content, ContentFormat format)
    {
        FormattedText text = new(content, format);
        return text.IsEmpty ? Empty : text;
    }

    public static class Rules
    {
        public const int ContentMaxLength = 10_000;
    }
}
