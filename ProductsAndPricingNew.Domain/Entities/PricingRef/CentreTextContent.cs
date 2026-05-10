using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

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
}