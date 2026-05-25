using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Discounts;

/// <summary>
/// A configured discount. The legacy entity carried applicability as columns (schools,
/// booking-creation dates, product dates) plus two separate <c>ICriteria</c> — a basket-level
/// one and a per-action row-level one — because the old criteria engine was too slow to be the
/// primary filter. In the NRules model the flat product-row fact carries booking-level fields
/// too, so every one of those conditions is now expressed as rules in
/// <see cref="ShouldApplyRuleset"/>. The entity keeps only <see cref="DivisionId"/>, the coarse
/// key the engine bundles rulesets by. The discount's monetary effect (banded action values) is
/// pricing data and is modelled in a later phase, alongside the fee pricing data.
/// </summary>
public sealed class Discount : AggregateRoot<int>
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public int DivisionId { get; private set; }
    public DiscountType Type { get; private set; }
    public bool IsActive { get; private set; }
    public RulesetRef ShouldApplyRuleset { get; private set; } = RulesetRef.None;

    private Discount() { }

    public static Discount Create(string name, int divisionId, DiscountType type)
    {
        Discount discount = new()
        {
            DivisionId = Guard.PositiveId(divisionId, nameof(DivisionId)),
            Type = type,
            IsActive = true
        };

        discount.Rename(name);

        return discount;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithDescription(string? description) =>
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

    public void WithType(DiscountType type) => Type = type;

    public void WithShouldApplyRuleset(RulesetRef ruleset) => ShouldApplyRuleset = ruleset;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public static class Rules
    {
        public const int NameMaxLength = 200;
        public const int DescriptionMaxLength = 1000;
    }
}
