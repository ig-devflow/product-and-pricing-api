using System.Collections.Concurrent;
using NRules;

namespace ProductsAndPricingNew.Pricing.Engine;

/// <summary>In-memory <see cref="IRuleSessionCache"/>. Building a session factory compiles
/// the Rete network, so it is cached and rebuilt only when the bundle version changes.</summary>
public sealed class RuleSessionCache : IRuleSessionCache
{
    private readonly ConcurrentDictionary<string, Entry> _entries = new();

    public ISessionFactory GetOrCreate(string key, int version, Func<ISessionFactory> factory)
    {
        Entry entry = _entries.AddOrUpdate(
            key,
            _ => new Entry(version, factory()),
            (_, existing) => existing.Version == version ? existing : new Entry(version, factory()));

        return entry.Factory;
    }

    private sealed record Entry(int Version, ISessionFactory Factory);
}
