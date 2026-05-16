using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public abstract class TextContent : Entity<int>
{
    public int ContentTemplateId { get; protected set; }
    public int? AudienceId { get; protected set; }
    public FormattedText Text { get; protected set; } = FormattedText.Empty;
    public bool IsDeleted { get; protected set; }

    protected TextContent() { }

    protected TextContent(int contentTemplateId, int? audienceId, FormattedText text)
    {
        if (contentTemplateId <= 0)
            throw new DomainException("Content template id must be provided.");

        ArgumentNullException.ThrowIfNull(text);

        if (text.IsEmpty)
            throw new DomainException("Text content cannot be empty.");

        ContentTemplateId = contentTemplateId;
        AudienceId = NormalizeAudienceId(audienceId);
        Text = text;
    }

    internal bool Matches(int contentTemplateId, int? audienceId)
        => ContentTemplateId == contentTemplateId &&
           AudienceId == NormalizeAudienceId(audienceId);

    internal void ChangeText(FormattedText text)
    {
        ArgumentNullException.ThrowIfNull(text);

        if (text.IsEmpty)
        {
            Delete();
            return;
        }

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
