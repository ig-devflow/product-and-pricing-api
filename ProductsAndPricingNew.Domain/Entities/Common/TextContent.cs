using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Domain.Entities.Common;

public abstract class TextContent : Entity<int>
{
    public abstract ContentTemplateScope OwnerScope { get; }
    public int ContentTemplateId { get; protected set; }
    public int? AudienceId { get; protected set; }
    public FormattedText Text { get; protected set; } = FormattedText.Empty;
    public bool IsDeleted { get; protected set; }

    protected TextContent()
    {
    }

    protected TextContent(int contentTemplateId, int? audienceId, FormattedText text)
    {
        if (contentTemplateId <= 0)
            throw new DomainException("Content template id must be provided.");

        ContentTemplateId = contentTemplateId;
        AudienceId = NormalizeAudienceId(audienceId);
        Text = text;
    }

    internal bool Matches(int contentTemplateId, int? audienceId)
        => ContentTemplateId == contentTemplateId &&
           AudienceId == NormalizeAudienceId(audienceId);

    internal void ChangeText(FormattedText text)
    {
        if (IsDeleted)
            Restore();

        Text = text;
    }

    internal void Delete()
    {
        IsDeleted = true;
    }

    internal void Restore()
    {
        IsDeleted = false;
    }

    private static int? NormalizeAudienceId(int? audienceId)
        => audienceId > 0 ? audienceId : null;
}