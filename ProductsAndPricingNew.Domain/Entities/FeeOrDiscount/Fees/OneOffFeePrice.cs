using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public sealed class OneOffFeePrice : Entity<int>
{
    public int Year { get; private set; }
    public int CurrencyId { get; private set; }
    public decimal Amount { get; private set; }

    private OneOffFeePrice() { }

    internal OneOffFeePrice(int year, int currencyId, decimal amount)
    {
        Year = year;
        CurrencyId = Guard.PositiveId(currencyId, nameof(CurrencyId));
        ChangeAmount(amount);
    }

    internal static OneOffFeePrice Create(int year, int currencyId, decimal amount)
    {

        return new OneOffFeePrice(year, currencyId, amount);
    }

    internal void ChangeAmount(decimal amount)
    {
        if (amount < 0m)
            throw new DomainException("A one-off fee amount cannot be negative.");

        Amount = amount;
    }
}