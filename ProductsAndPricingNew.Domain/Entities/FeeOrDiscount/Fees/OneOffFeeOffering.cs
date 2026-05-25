using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees.Definitions;
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

    public void WithPrices(IEnumerable<OneOffFeePriceDefinition> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);

        List<OneOffFeePriceDefinition> incoming = prices.ToList();
        var incomingKeys = new HashSet<(int Year, int CurrencyId)>();

        foreach (OneOffFeePriceDefinition def in incoming)
        {
            if (!Years.Includes(def.Year))
                throw new DomainException($"Pricing year {def.Year} is outside the offering's active years.");

            Guard.PositiveId(def.CurrencyId, nameof(def.CurrencyId));

            if (!incomingKeys.Add((def.Year, def.CurrencyId)))
                throw new DomainException($"Duplicate price for year {def.Year} and currency {def.CurrencyId}.");
        }

        _prices.RemoveAll(p => !incomingKeys.Contains((p.Year, p.CurrencyId)));

        foreach (OneOffFeePriceDefinition def in incoming)
        {
            OneOffFeePrice? existing = _prices.SingleOrDefault(p => p.Year == def.Year && p.CurrencyId == def.CurrencyId);

            if (existing is null)
                _prices.Add(new OneOffFeePrice(def.Year, def.CurrencyId, def.Amount));
            else
                existing.WithAmount(def.Amount);
        }
    }
}
