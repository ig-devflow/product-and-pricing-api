using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class ContentTemplate : Entity<int>, ISoftDeletable
{
    public string Name { get; init; }
    public string? Description { get; init; }
    public ContentTemplateScope Scope { get; init; }
    public bool IsDeleted { get; init; }

    private ContentTemplate()
    {
    }

    public void EnsureActive(ContentTemplateScope ownerScope)
    {
        if (IsDeleted)
            throw new DomainException($"Content template '{Name}' is deleted.");

        if (Scope != ownerScope)
            throw new DomainException($"Content template '{Name}' cannot be used for {ownerScope}.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 200;
        public const int DescriptionMaxLength = 500;
    }
}