using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.Domain.Entities.ReferenceData;

public sealed class Audience : Entity<int>
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
}