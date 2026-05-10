using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public sealed class Currency : Entity<int>, ISoftDeletable
{
    public string Name { get; init; }
    public string IsoCode { get; init; }
    public char Symbol { get; init; }
    public bool IsDeleted { get; init; }

    private Currency()
    {
    }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"Currency '{IsoCode}' is deleted.");
    }

    public static class Rules
    {
        public const int IsoCodeMaxLength = 3;
        public const int NameMaxLength = 100;
        public const int SymbolMaxLength = 1;
    }
}
