using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Country : Entity<int>, ISoftDeletable
{
    public string Code { get; init; }
    public string Name { get; init; }
    public bool IsDeleted { get; init; }

    private Country()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Country '{Name}' is deleted.");
    }
}
