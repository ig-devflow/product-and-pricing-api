using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;

public sealed class CancellationFeeOffering : AggregateRoot<int>
{
    public int SchoolId { get; private set; }
    public int CancellationFeeId { get; private set; }
    public ActivePricingYears Years { get; private set; }
    public RulesetRef ApplyRuleset { get; private set; } = RulesetRef.None;
    public RulesetRef ChargeRuleset { get; private set; } = RulesetRef.None;

    private CancellationFeeOffering() { }

    public static CancellationFeeOffering Create(
        int id,
        int schoolId,
        int cancellationFeeId,
        ActivePricingYears years)
    {
        return new CancellationFeeOffering
        {
            Id = id,
            SchoolId = schoolId,
            CancellationFeeId = cancellationFeeId,
            Years = years
        };
    }

    public void ChangeApplyRuleset(RulesetRef rs) => ApplyRuleset = rs;

    public void ChangeChargeRuleset(RulesetRef rs) => ChargeRuleset = rs;

    public void ExtendYears(int year) => Years = Years.Extend(year);
}