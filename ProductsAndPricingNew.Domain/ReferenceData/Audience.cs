using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Audience : Entity<int>, ISoftDeletable
{
    public string Name { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    private Audience()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Audience '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
