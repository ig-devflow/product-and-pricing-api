using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.Domain.Entities.ReferenceData;

public sealed class ContentTemplate : Entity<int>
{
    public string Name { get; private set; } = null!;
    public string? UsageDescription { get; private set; }
    public ContentTemplateScope Scope { get; private set; }
    public bool IsDeleted { get; private set; }

    private ContentTemplate()
    {
    }

    public void EnsureCanBeUsedFor(ContentTemplateScope ownerScope)
    {
        if (IsDeleted)
            throw new DomainException($"Content template '{Name}' is deleted.");

        if (Scope != ownerScope)
            throw new DomainException($"Content template '{Name}' cannot be used for {ownerScope}.");
    }
}