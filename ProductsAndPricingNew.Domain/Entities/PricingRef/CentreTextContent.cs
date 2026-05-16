using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreTextContent : TextContent
{
    public int CentreId { get; private set; }

    private CentreTextContent()
    {
    }

    private CentreTextContent(int contentTemplateId, int? audienceId, FormattedText text)
        : base(contentTemplateId, audienceId, text)
    {
    }

    internal static CentreTextContent Create(TextContentDefinition definition, FormattedText text)
        => new(definition.ContentTemplateId, definition.NormalizedAudienceId, text);
}