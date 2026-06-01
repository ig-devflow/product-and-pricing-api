using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.ReferenceData;

public class CourseLanguage : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public bool IsDeleted { get; init; }

    private CourseLanguage() { }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"CourseLanguage '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
