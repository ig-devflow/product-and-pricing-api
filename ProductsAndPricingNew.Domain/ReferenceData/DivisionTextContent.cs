using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class DivisionTextContent : TextContent
{
    public int DivisionId { get; init; }

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
