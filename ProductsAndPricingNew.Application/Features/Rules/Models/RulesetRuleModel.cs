namespace ProductsAndPricingNew.Application.Features.Rules.Models;

/// <summary>One rule within a ruleset write operation: its name, priority and authored definition.</summary>
public sealed record RulesetRuleModel(
    string Name,
    int Priority,
    RuleGroupModel Definition);
