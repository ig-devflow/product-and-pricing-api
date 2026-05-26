using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class CentreContactType : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; init; }

    private CentreContactType() { }

    public static class Rules
    {
        public const int NameMaxLength = 50;
    }
}