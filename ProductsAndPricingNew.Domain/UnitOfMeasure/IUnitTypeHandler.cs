using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// The arithmetic implied by a <see cref="UnitCalculationKind"/>: normalising unit counts and
/// walking dates. Replaces the legacy <c>IUnitsDateHandler</c>. Unit-period and period-grouping
/// construction is a price-calculation concern and is added in a later phase.
/// </summary>
public interface IUnitTypeHandler
{
    UnitCalculationKind Kind { get; }

    /// <summary>Carries overflowing minor units up into major units (e.g. 8 days → 1 week, 1 day).</summary>
    void Normalize(ref int major, ref int minor);

    DateOnly AddTo(DateOnly date, int major, int minor);

    DateOnly SubtractFrom(DateOnly date, int major, int minor);

    (int major, int minor) Between(DateRange range, UnitsRoundingMode rounding);

    string Format(int major, int minor);
}
