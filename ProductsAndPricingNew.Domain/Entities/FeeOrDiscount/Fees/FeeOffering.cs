using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public abstract class FeeOffering : AggregateRoot<int>
{
    public int SchoolId { get; private set; }
    public int FeeId { get; private set; }
    public ActivePricingYears Years { get; protected set; }
    public RulesetRef ApplyRuleset { get; private set; } = RulesetRef.None;
    public RulesetRef ApplicableRowRuleset { get; private set; } = RulesetRef.None;

    public abstract FeePricingKind Kind { get; }

    private protected FeeOffering() { }

    private protected FeeOffering(int schoolId, int feeId, ActivePricingYears years)
    {
        SchoolId = schoolId;
        FeeId = feeId;
        Years = years;
    }

    public void ChangeApplyRuleset(RulesetRef ruleset) => ApplyRuleset = ruleset;

    public void ChangeApplicableRowRuleset(RulesetRef ruleset) => ApplicableRowRuleset = ruleset;

    public void ExtendYears(int year) => Years = Years.Extend(year);
}