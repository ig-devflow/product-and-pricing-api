using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.Offerings.Definitions;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Offerings;

/// <summary>
/// A product made available at a school. Unlike the legacy model, the offering OWNS its schedule:
/// the date strategy is no longer a separate aggregate that can drift away from the offering.
/// Yearly pricing lives in <c>OfferingYearPricing</c> and is validated against this offering's
/// schedule. The product's <see cref="UnitType"/> gates which <see cref="ScheduleKind"/>s are legal.
/// </summary>
public sealed class ProductOffering : AggregateRoot<int>
{
    private readonly List<ScheduleSegment> _scheduleSegments = new();

    public int SchoolId { get; private set; }
    public ProductRef Product { get; private set; }
    public int UnitTypeId { get; private set; }
    public bool UnitTypeIsDateBased { get; private set; }
    public OfferingClosure Closure { get; private set; } = OfferingClosure.Open;

    public IReadOnlyCollection<ScheduleSegment> ScheduleSegments => _scheduleSegments.AsReadOnly();

    private ProductOffering() { }

    public void AddScheduleSegment(ScheduleSegmentDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        EnsureKindAllowed(definition.Kind);

        var segment = new ScheduleSegment(definition);
        EnsureNoOverlap(segment);

        _scheduleSegments.Add(segment);
        RecomputeDateStrategiesClosure();
    }

    public void CloseCurrentSegment(DateOnly endDate)
    {
        ScheduleSegment current = _scheduleSegments
                                      .OrderByDescending(s => s.From)
                                      .FirstOrDefault(s => s.IsOpenEnded)
                                  ?? throw new DomainException("There is no open-ended schedule segment to close.");

        current.Close(endDate);
        RecomputeDateStrategiesClosure();
    }

    public void RemoveScheduleSegment(DateOnly from)
    {
        ScheduleSegment segment = _scheduleSegments.FirstOrDefault(s => s.From == from)
            ?? throw new DomainException($"No schedule segment starts on {from:yyyy-MM-dd}.");

        _scheduleSegments.Remove(segment);
        RecomputeDateStrategiesClosure();
    }

    public void ChangeDecommission(DateOnly? date) =>
        Closure.WithDecommission(date);

    /// <summary>
    /// True when a single schedule segment covers the whole range. The yearly pricing aggregate
    /// uses this to reject price periods that fall outside the offering's schedule.
    /// </summary>
    public bool ScheduleCovers(DateRange range) =>
        _scheduleSegments.Any(s => s.Covers(range.Start) && s.Covers(range.End));

    private void EnsureKindAllowed(ScheduleKind kind)
    {
        if (!UnitTypeIsDateBased && kind is not (ScheduleKind.Continuous or ScheduleKind.FixedRange))
            throw new DomainException("A non-date-based offering can only use Continuous or FixedRange scheduling.");
    }

    private void EnsureNoOverlap(ScheduleSegment candidate)
    {
        if (_scheduleSegments.Any(existing => existing.Overlaps(candidate.From, candidate.EffectiveEnd)))
            throw new DomainException("The schedule segment overlaps an existing segment.");
    }

    private void RecomputeDateStrategiesClosure()
    {
        DateOnly? closure =
            _scheduleSegments.Count == 0 || _scheduleSegments.Any(s => s.IsOpenEnded)
                ? null
                : _scheduleSegments.Max(s => s.EffectiveEnd);

        Closure = Closure.WithStrategies(closure);
    }

    public sealed class Builder
    {
        private readonly int _schoolId;
        private readonly ProductRef _product;
        private readonly UnitType _unitType;
        private readonly List<ScheduleSegmentDefinition> _segments = new();

        public Builder(int schoolId, ProductRef product, UnitType unitType)
        {
            ArgumentNullException.ThrowIfNull(unitType);

            _schoolId = Guard.PositiveId(schoolId, nameof(SchoolId));
            _product = product.Id > 0
                ? product
                : throw new DomainException("Product reference must have a positive id.");
            _unitType = unitType;

            UnitTypePolicy.EnsureAllowedForProduct(product.Kind, unitType);
        }

        public Builder WithScheduleSegment(ScheduleSegmentDefinition definition)
        {
            ArgumentNullException.ThrowIfNull(definition);
            _segments.Add(definition);
            return this;
        }

        public ProductOffering Build()
        {
            ProductOffering offering = new()
            {
                SchoolId = _schoolId,
                Product = _product,
                UnitTypeId = _unitType.Id,
                UnitTypeIsDateBased = _unitType.IsDateBased
            };

            foreach (ScheduleSegmentDefinition segment in _segments)
                offering.AddScheduleSegment(segment);

            return offering;
        }
    }
}