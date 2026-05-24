using NRules;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Pricing.Compilation;

namespace ProductsAndPricingNew.Pricing.Engine;

/// <summary>
/// Evaluates a bundle of rulesets against a set of facts with NRules: obtains the compiled
/// session factory from the cache (compiling it on first use or after a version change),
/// fires a session over the facts, and returns every rule match.
/// </summary>
public sealed class NRulesRuleEvaluator
{
    private readonly RulesetCompiler _compiler;
    private readonly IRuleSessionCache _cache;

    public NRulesRuleEvaluator(RulesetCompiler compiler, IRuleSessionCache cache)
    {
        _compiler = compiler;
        _cache = cache;
    }

    public IReadOnlyList<RuleMatch> Evaluate(IReadOnlyList<Ruleset> rulesets, IReadOnlyList<RuleFact> facts)
    {
        if (rulesets.Count == 0 || facts.Count == 0)
            return [];

        string key = string.Join(",", rulesets.Select(ruleset => ruleset.Id));
        int version = rulesets.Sum(ruleset => ruleset.RuleVersion);

        ISessionFactory factory = _cache.GetOrCreate(key, version, () => _compiler.Compile(rulesets));

        ISession session = factory.CreateSession();
        session.InsertAll(facts);
        session.Fire();

        return session.Query<RuleMatch>().ToList();
    }
}
