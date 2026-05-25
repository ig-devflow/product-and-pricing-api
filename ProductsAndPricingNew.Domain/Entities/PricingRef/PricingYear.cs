using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class PricingYear : AggregateRoot<int>
{
    public int DivisionId { get; private set; }
    public int Year { get; private set; }
    public RulesetRef EarlyBirdRuleset { get; private set; } = RulesetRef.None;

    private PricingYear() { }

    private PricingYear(int divisionId, int year)
    {
        DivisionId = divisionId;
        Year = year;
    }

    public static PricingYear Create(int divisionId, int year)
    {
        if (year <= 0)
            throw new DomainException("Pricing year must be greater than zero.");

        Guard.PositiveId(divisionId, nameof(DivisionId));

        return new PricingYear(divisionId, year);
    }

    public void WithEarlyBirdRuleset(RulesetRef ruleset) =>
        EarlyBirdRuleset = ruleset;

    public BusinessYear AsBusinessYear() => BusinessYear.Of(Year);

    public DateRange GetBusinessYearDateRange() => AsBusinessYear().ToDateRange();
}