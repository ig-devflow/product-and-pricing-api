using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public sealed class SupplementFeeOffering : FeeOffering
{
    private readonly List<SupplementPrice> _prices = new();

    public bool HasSpecificDates { get; private set; }
    public int? MaximumUnits { get; private set; }

    public IReadOnlyCollection<SupplementPrice> Prices => _prices.AsReadOnly();

    public override FeePricingKind Kind => FeePricingKind.Supplement;

    private SupplementFeeOffering() { }

    private SupplementFeeOffering(int schoolId, int feeId, ActivePricingYears years)
        : base(schoolId, feeId, years)
    {
    }

    public static SupplementFeeOffering Create(int schoolId, Fee fee, UnitType feeUnitType, ActivePricingYears years)
    {
        ArgumentNullException.ThrowIfNull(fee);
        ArgumentNullException.ThrowIfNull(feeUnitType);

        if (feeUnitType.Id != fee.UnitTypeId)
            throw new DomainException("The supplied unit type does not match the fee's unit type.");

        if (!feeUnitType.IsDateBased)
            throw new DomainException("A supplement fee offering requires a fee with a date-based unit type.");

        return new SupplementFeeOffering(schoolId, fee.Id, years);
    }

    public void SetMaximumUnits(int? maximumUnits)
    {
        if (maximumUnits is < 1)
            throw new DomainException("Maximum units must be at least 1.");

        MaximumUnits = maximumUnits;
    }

    public void UseAllDatesPricing()
    {
        HasSpecificDates = false;
        _prices.RemoveAll(p => !p.IsAllDates);
    }

    public void UseSpecificDatesPricing()
    {
        HasSpecificDates = true;
        _prices.RemoveAll(p => p.IsAllDates);
    }

    public void SetAllDatesPrice(int year, int currencyId, decimal pricePerMajorUnit, decimal pricePerMinorUnit)
    {
        if (HasSpecificDates)
            throw new DomainException("This supplement uses specific-date pricing; call SetPeriodPrice instead.");

        Upsert(year, currencyId, periodStart: null, periodEnd: null, pricePerMajorUnit, pricePerMinorUnit);
    }

    public void SetPeriodPrice(int year, DateOnly periodStart, DateOnly periodEnd, int currencyId, decimal pricePerMajorUnit, decimal pricePerMinorUnit)
    {
        if (!HasSpecificDates)
            throw new DomainException("This supplement uses all-dates pricing; call SetAllDatesPrice instead.");

        Upsert(year, currencyId, periodStart, periodEnd, pricePerMajorUnit, pricePerMinorUnit);
    }

    public void RemovePrice(int year, int currencyId, DateOnly? periodStart, DateOnly? periodEnd)
    {
        SupplementPrice? existing = _prices.SingleOrDefault(p => p.Matches(year, currencyId, periodStart, periodEnd));

        if (existing is not null)
            _prices.Remove(existing);
    }

    private void Upsert(int year, int currencyId, DateOnly? periodStart, DateOnly? periodEnd, decimal pricePerMajorUnit, decimal pricePerMinorUnit)
    {
        if (!Years.Includes(year))
            throw new DomainException($"Pricing year {year} is outside the offering's active years.");

        Guard.PositiveId(currencyId, nameof(currencyId));

        SupplementPrice? existing = _prices.SingleOrDefault(p => p.Matches(year, currencyId, periodStart, periodEnd));

        if (existing is null)
            _prices.Add(new SupplementPrice(year, currencyId, pricePerMajorUnit, pricePerMinorUnit, periodStart, periodEnd));
        else
            existing.ChangePrice(pricePerMajorUnit, pricePerMinorUnit);
    }
}