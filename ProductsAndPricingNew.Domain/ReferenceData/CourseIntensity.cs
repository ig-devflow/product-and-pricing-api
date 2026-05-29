using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public class CourseIntensity : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; init; }

    private CourseIntensity() { }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"CourseIntensity '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
