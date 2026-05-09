using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Currency : Entity<int>, ISoftDeletable
{
    public string IsoCode { get; private set; } = null!;
    public char Symbol { get; private set; }
    public bool IsDeleted { get; private set; }

    private Currency()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Currency '{IsoCode}' is deleted.");
    }
}
