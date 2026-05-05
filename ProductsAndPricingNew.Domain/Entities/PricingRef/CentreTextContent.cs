using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreTextContent : TextContent
{
    public int CentreId { get; private set; }

    private CentreTextContent()
    {
    }

    internal CentreTextContent(int contentTemplateId, int? audienceId, FormattedText text)
        : base(contentTemplateId, audienceId, text)
    {
    }

    public override ContentTemplateScope OwnerScope => ContentTemplateScope.Centre;
}