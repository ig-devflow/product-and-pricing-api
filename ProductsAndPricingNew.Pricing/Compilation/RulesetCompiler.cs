using System.Linq.Expressions;
using NRules;
using NRules.RuleModel;
using NRules.RuleModel.Builders;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Pricing.Engine;

namespace ProductsAndPricingNew.Pricing.Compilation;

/// <summary>
/// Compiles the rules of one or more <see cref="Ruleset"/>s into an NRules
/// <see cref="ISessionFactory"/>. Each active <see cref="PricingRule"/> becomes one NRules
/// rule: its JSON specification is deserialized, compiled into a condition over a
/// <see cref="RuleFact"/>, and a matching fact triggers an action inserting a
/// <see cref="RuleMatch"/>.
/// </summary>
public sealed class RulesetCompiler
{
    public ISessionFactory Compile(IEnumerable<Ruleset> rulesets)
    {
        List<IRuleDefinition> definitions = [];
        int index = 0;

        foreach (Ruleset ruleset in rulesets)
        {
            foreach (PricingRule rule in ruleset.Rules)
            {
                if (rule.IsActive)
                    definitions.Add(BuildRule(ruleset.Id, rule, index++));
            }
        }

        return new RuleCompiler().Compile(definitions);
    }

    private static IRuleDefinition BuildRule(int rulesetId, PricingRule rule, int index)
    {
        RuleNode ast = RuleSpecJson.Deserialize(rule.ScriptJson);
        Expression<Func<RuleFact, bool>> condition = RuleNodeExpressionCompiler.Compile(ast);
        string ruleName = rule.Name;

        RuleBuilder builder = new();
        builder.Name($"ruleset{rulesetId}.rule{index}.{ruleName}");
        builder.Priority(rule.Priority);

        PatternBuilder pattern = builder.LeftHandSide().Pattern(typeof(RuleFact), "fact");
        pattern.Condition(condition);

        Expression<Action<IContext, RuleFact>> action =
            (ctx, fact) => ctx.Insert(new RuleMatch(rulesetId, ruleName, fact));
        builder.RightHandSide().Action(action);

        return builder.Build();
    }
}
