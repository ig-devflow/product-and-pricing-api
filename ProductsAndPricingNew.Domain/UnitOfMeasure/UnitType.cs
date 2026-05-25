using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// The unit a product or fee is priced in — reference data. The set lives in the database (a new
/// unit type is an inserted row, not a code change); the per-kind arithmetic is code, resolved
/// through <see cref="IUnitTypeHandler"/> by <see cref="CalculationKind"/>.
/// </summary>
public sealed class UnitType : Entity<int>, ISoftDeletable
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public bool IsDateBased { get; init; }
    public UnitCalculationKind CalculationKind { get; init; }
    public int MinMajorUnits { get; init; }
    public int MinMinorUnits { get; init; }
    public bool IsDeleted { get; init; }

    private UnitType() { }

    public void EnsureActive()
    {
        if (IsDeleted)
            throw new DomainException($"UnitType '{Name}' is deleted.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int DescriptionMaxLength = 200;
    }
}
