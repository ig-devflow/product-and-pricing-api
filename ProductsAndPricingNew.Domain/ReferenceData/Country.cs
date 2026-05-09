using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Country : Entity<int>, ISoftDeletable
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsDeleted { get; private set; }

    private Country()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Country '{Name}' is deleted.");
    }
}
