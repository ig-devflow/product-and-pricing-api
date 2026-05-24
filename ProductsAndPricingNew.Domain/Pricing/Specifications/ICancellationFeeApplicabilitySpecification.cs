using ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;
using ProductsAndPricingNew.Domain.Pricing.Engine;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications;

public interface ICancellationFeeApplicabilitySpecification
{
    Task<bool> ShouldApplyAsync(CancellationFeeOffering offering, RuleEvaluationContext context, CancellationToken ct = default);
    Task<bool> ShouldChargeAsync(CancellationFeeOffering offering, RuleEvaluationContext context, CancellationToken ct = default);
}
