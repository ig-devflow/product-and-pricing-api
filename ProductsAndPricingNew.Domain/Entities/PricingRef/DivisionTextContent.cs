using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class DivisionTextContent : TextContent
{
    public int DivisionId { get; private set; }

    private DivisionTextContent()
    {
    }

    internal DivisionTextContent(int contentTemplateId, int? audienceId, FormattedText text)
        : base(contentTemplateId, audienceId, text)
    {
    }

    public override ContentTemplateScope OwnerScope => ContentTemplateScope.Division;
}