using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public class AccommodationBoardType : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; init; }

    private AccommodationBoardType() { }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"AccommodationBoardType '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}