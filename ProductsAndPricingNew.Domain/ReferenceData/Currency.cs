using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Currency : Entity<int>, ISoftDeletable
{
    public string Name { get; }
    public string IsoCode { get; } = null!;
    public char Symbol { get; }
    public bool IsDeleted { get; }

    private Currency()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Currency '{IsoCode}' is deleted.");
    }
}
