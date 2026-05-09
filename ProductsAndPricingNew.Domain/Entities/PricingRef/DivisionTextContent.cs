using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class DivisionTextContent : TextContent
{
    public int DivisionId { get; private set; }

    private DivisionTextContent()
    {
    }

    private DivisionTextContent(int contentTemplateId, int? audienceId, FormattedText text)
        : base(contentTemplateId, audienceId, text)
    {
    }

    internal static DivisionTextContent Create(TextContentDefinition definition, FormattedText text)
        => new(definition.ContentTemplateId, definition.NormalizedAudienceId, text);
}
