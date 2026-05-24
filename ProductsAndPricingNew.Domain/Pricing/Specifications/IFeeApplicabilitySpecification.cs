using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;
using ProductsAndPricingNew.Domain.Pricing.Engine;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications;

public interface IFeeApplicabilitySpecification
{
    Task<bool> ShouldApplyAsync(FeeOffering offering, RuleEvaluationContext context, CancellationToken ct = default);
    Task<bool> IsApplicableRowAsync(FeeOffering offering, RuleEvaluationContext context, CancellationToken ct = default);
}
