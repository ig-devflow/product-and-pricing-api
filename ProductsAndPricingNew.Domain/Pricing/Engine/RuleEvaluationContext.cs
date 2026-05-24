using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Domain.Pricing.Engine;

public sealed class RuleEvaluationContext
{
    public required int SchoolId { get; init; }
    public required int DivisionId { get; init; }
    public required int PricingYear { get; init; }
    public required ProductRef Product { get; init; }
    public IReadOnlyDictionary<string, object?> ExtraFields { get; init; } = new Dictionary<string, object?>();
}
