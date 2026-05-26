using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct ActivePricingYears
{
    public int From { get; }
    public int? To { get; private init; }

    private ActivePricingYears(int from, int? to)
    {
        From = from;
        To = to;
    }

    public static ActivePricingYears Create(int from, int? to)
    {
        if (from <= 0)
            throw new DomainException("Pricing year From must be greater than zero.");

        if (to.HasValue && to.Value < from)
            throw new DomainException("Pricing year To must be greater than or equal to From.");

        return new ActivePricingYears(from, to);
    }

    public bool Includes(int year) =>
        year >= From && (!To.HasValue || year <= To.Value);

    public ActivePricingYears Extend(int year)
    {
        if (year < From)
            throw new DomainException("Cannot extend pricing years below the From year.");

        return this with { To = year };
    }
}
