using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.UnitOfMeasure;

/// <summary>
/// The fixed set of unit-type handlers, indexed by kind. A new unit type with existing behaviour
/// needs no change here — only a new <i>kind</i> of behaviour does.
/// </summary>
public sealed class UnitTypeHandlerRegistry : IUnitTypeHandlerRegistry
{
    private readonly Dictionary<UnitCalculationKind, IUnitTypeHandler> _handlers;

    public UnitTypeHandlerRegistry()
    {
        IUnitTypeHandler[] handlers =
        [
            new NonDateBasedUnitTypeHandler(UnitCalculationKind.FixedPrice),
            new NonDateBasedUnitTypeHandler(UnitCalculationKind.Any),
            new DayUnitTypeHandler(),
            new CalendarWeekUnitTypeHandler(UnitCalculationKind.CalendarWeek),
            new CalendarWeekUnitTypeHandler(UnitCalculationKind.CalendarNightWeek),
            new WorkingWeekUnitTypeHandler(),
            new MonthUnitTypeHandler()
        ];

        _handlers = handlers.ToDictionary(handler => handler.Kind);
    }

    public IUnitTypeHandler Resolve(UnitCalculationKind kind) =>
        _handlers.TryGetValue(kind, out IUnitTypeHandler? handler)
            ? handler
            : throw new DomainException($"No unit-type handler is registered for kind {kind}.");
}