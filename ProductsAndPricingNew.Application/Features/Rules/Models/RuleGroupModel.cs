namespace ProductsAndPricingNew.Application.Features.Rules.Models;

/// <summary>
/// A UI-authored rule group: its conditions and sub-groups combined with <see cref="Logic"/>,
/// optionally negated. The root of an authored rule is a single group.
/// </summary>
public sealed record RuleGroupModel(
    RuleLogic Logic,
    bool Negate,
    IReadOnlyList<RuleConditionModel>? Conditions = null,
    IReadOnlyList<RuleGroupModel>? Groups = null);
