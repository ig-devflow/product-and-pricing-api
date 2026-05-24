namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Discounts;

/// <summary>
/// The kind of configured discount. The legacy <c>EarlyBird</c> type is a pricing-year selection
/// mechanism (not a discount) and <c>DeferredTuition</c> is a calculator-driven price match —
/// neither is a configurable discount, so both are intentionally excluded here.
/// </summary>
public enum DiscountType
{
    MarketDiscount = 1,
    SpecialOffer = 2
}
