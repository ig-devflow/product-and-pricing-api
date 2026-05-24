using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public sealed class OneOffFeeOffering : FeeOffering
{
    private readonly List<OneOffFeePrice> _prices = new();

    public IReadOnlyCollection<OneOffFeePrice> Prices => _prices.AsReadOnly();

    public override FeePricingKind Kind => FeePricingKind.OneOff;

    private OneOffFeeOffering() { }

    private OneOffFeeOffering(int schoolId, int feeId, ActivePricingYears years)
        : base(schoolId, feeId, years)
    {
    }

    public static OneOffFeeOffering Create(int schoolId, int feeId, ActivePricingYears years) =>
        new(schoolId, feeId, years);

    public void SetPrice(int year, int currencyId, decimal amount)
    {
        if (!Years.Includes(year))
            throw new DomainException($"Pricing year {year} is outside the offering's active years.");

        Guard.PositiveId(currencyId, nameof(currencyId));

        OneOffFeePrice? existing = _prices.SingleOrDefault(p => p.Year == year && p.CurrencyId == currencyId);

        if (existing is null)
            _prices.Add(new OneOffFeePrice(year, currencyId, amount));
        else
            existing.ChangeAmount(amount);
    }

    public void RemovePrice(int year, int currencyId)
    {
        OneOffFeePrice? existing = _prices.SingleOrDefault(p => p.Year == year && p.CurrencyId == currencyId);

        if (existing is not null)
            _prices.Remove(existing);
    }
}