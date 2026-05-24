using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct RulesetRef
{
    private int RulesetId { get; }

    public static readonly RulesetRef None = new(0);
    public bool IsSet => RulesetId > 0;

    private RulesetRef(int rulesetId)
    {
        RulesetId = rulesetId;
    }

    public static RulesetRef Create(int rulesetId)
    {
        Guard.PositiveId(rulesetId, nameof(rulesetId));
        return new RulesetRef(rulesetId);
    }
}