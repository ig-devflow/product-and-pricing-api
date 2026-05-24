using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// Allowed <see cref="UnitCalculationKind"/>s per product/fee subject. Mirrors the legacy
/// OfferingService rules but pulled up to the product invariant, so a product can never be
/// persisted with a UnitType its kind disallows. <see cref="UnitCalculationKind.Any"/> is
/// reserved for legacy Discount placeholders and is rejected for offerings/products.
/// </summary>
public static class UnitTypePolicy
{
    private static readonly Dictionary<ProductKind, HashSet<UnitCalculationKind>> ProductRules = new()
    {
        [ProductKind.Course] =
        [
            UnitCalculationKind.CalendarWeek,
            UnitCalculationKind.WorkingWeek,
            UnitCalculationKind.Day
        ],

        [ProductKind.AccommodationRoom] =
        [
            UnitCalculationKind.CalendarWeek,
            UnitCalculationKind.CalendarNightWeek,
            UnitCalculationKind.WorkingWeek,
            UnitCalculationKind.Day
        ],

        [ProductKind.AddOn] =
        [
            UnitCalculationKind.FixedPrice,
            UnitCalculationKind.CalendarWeek,
            UnitCalculationKind.WorkingWeek,
            UnitCalculationKind.Day
        ],

        [ProductKind.Transfer] =
        [
            UnitCalculationKind.FixedPrice
        ],

        [ProductKind.Package] =
        [
            UnitCalculationKind.CalendarWeek
        ],
    };

    private static readonly HashSet<UnitCalculationKind> FeeRules =
    [
        UnitCalculationKind.FixedPrice
    ];

    private static readonly HashSet<UnitCalculationKind> CancellationFeeRules =
    [
        UnitCalculationKind.FixedPrice
    ];

    public static void EnsureAllowedForProduct(ProductKind product, UnitType unitType)
    {
        ArgumentNullException.ThrowIfNull(unitType);

        unitType.EnsureActive();
        EnsureNotAny(unitType);

        if (!ProductRules.TryGetValue(product, out HashSet<UnitCalculationKind>? allowed))
            throw new DomainException($"No UnitType policy defined for {product}.");

        if (!allowed.Contains(unitType.CalculationKind))
            throw new DomainException(
                $"UnitType '{unitType.Name}' ({unitType.CalculationKind}) is not allowed for {product}. " +
                $"Allowed: {string.Join(", ", allowed)}.");
    }

    public static void EnsureAllowedForFee(UnitType unitType)
    {
        ArgumentNullException.ThrowIfNull(unitType);

        unitType.EnsureActive();
        EnsureNotAny(unitType);

        if (!FeeRules.Contains(unitType.CalculationKind))
            throw new DomainException(
                $"UnitType '{unitType.Name}' ({unitType.CalculationKind}) is not allowed for Fee. " +
                $"Allowed: {string.Join(", ", FeeRules)}.");
    }

    public static void EnsureAllowedForCancellationFee(UnitType unitType)
    {
        ArgumentNullException.ThrowIfNull(unitType);

        unitType.EnsureActive();
        EnsureNotAny(unitType);

        if (!CancellationFeeRules.Contains(unitType.CalculationKind))
            throw new DomainException(
                $"UnitType '{unitType.Name}' ({unitType.CalculationKind}) is not allowed for CancellationFee. " +
                $"Allowed: {string.Join(", ", CancellationFeeRules)}.");
    }

    private static void EnsureNotAny(UnitType unitType)
    {
        if (unitType.CalculationKind == UnitCalculationKind.Any)
            throw new DomainException("UnitType.Any is reserved for legacy Discount placeholders and must not be used on offerings or products.");
    }
}