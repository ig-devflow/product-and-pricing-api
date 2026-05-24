using NRules;

namespace ProductsAndPricingNew.Pricing.Engine;

/// <summary>
/// Caches compiled NRules <see cref="ISessionFactory"/> instances so a bundle of rulesets is
/// compiled once and recompiled only when its version changes.
/// </summary>
public interface IRuleSessionCache
{
    ISessionFactory GetOrCreate(string key, int version, Func<ISessionFactory> factory);
}
