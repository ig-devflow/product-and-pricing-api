using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Domain.SharedKernel.TextContent;

public readonly record struct TextContentDefinition
{
    public int ContentTemplateId { get; }
    public int? AudienceId { get; }
    public string? Content { get; }
    public ContentFormat Format { get; }

    public TextContentDefinition(int contentTemplateId, int? audienceId, string? content, ContentFormat format)
    {
        ContentTemplateId = contentTemplateId;
        AudienceId = audienceId;
        Content = content;
        Format = format;
    }

    public int? NormalizedAudienceId => AudienceId > 0 ? AudienceId : null;
    public bool IsEmpty => string.IsNullOrWhiteSpace(Content);
    public (int ContentTemplateId, int? AudienceId) Key
    {
        get
        {
            EnsureValidKey();
            return (ContentTemplateId, NormalizedAudienceId);
        }
    }

    public void EnsureValid()
    {
        EnsureValidKey();

        bool hasContent = !string.IsNullOrWhiteSpace(Content);

        if (!hasContent && Format != ContentFormat.None)
            throw new DomainException("Content format must be None when content is empty.");

        if (hasContent && Format == ContentFormat.None)
            throw new DomainException("Content format must be provided when content is not empty.");

        if (!Enum.IsDefined(Format))
            throw new DomainException($"Unsupported content format '{Format}'.");
    }

    public void EnsureValidKey()
    {
        if (ContentTemplateId <= 0)
            throw new DomainException("Content template id must be provided.");
    }
}
