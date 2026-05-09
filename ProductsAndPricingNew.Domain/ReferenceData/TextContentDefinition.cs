using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public readonly record struct TextContentDefinition
{
    public TextContentDefinition(int contentTemplateId, int? audienceId, string? content, ContentFormat format)
    {
        ContentTemplateId = contentTemplateId;
        AudienceId = audienceId;
        Content = content;
        Format = format;
    }

    public int ContentTemplateId { get; }
    public int? AudienceId { get; }
    public string? Content { get; }
    public ContentFormat Format { get; }

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

    public void EnsureValidKey()
    {
        if (ContentTemplateId <= 0)
            throw new DomainException("Content template id must be provided.");
    }
}
