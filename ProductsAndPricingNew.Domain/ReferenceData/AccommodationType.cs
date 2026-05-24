using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class AccommodationType : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public char Prefix { get; init; }
    public bool IsDeleted { get; init; }

    private AccommodationType() { }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"AccommodationType '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int PrefixMaxLength = 1;
    }
}