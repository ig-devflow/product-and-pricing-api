using ProductsAndPricingNew.Domain.Pricing.Engine;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications;

public interface IDiscountApplicabilitySpecification
{
    Task<bool> ShouldApplyAsync(int discountOfferingId, RuleEvaluationContext context, CancellationToken ct = default);
}
