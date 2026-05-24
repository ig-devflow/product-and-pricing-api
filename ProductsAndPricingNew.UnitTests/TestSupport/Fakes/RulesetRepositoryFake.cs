using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Repositories;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class RulesetRepositoryFake : IRulesetRepository
{
    public int AddCalls { get; private set; }
    public int RemoveCalls { get; private set; }
    public Ruleset? AddedRuleset { get; private set; }

    public Task<Ruleset?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(AddedRuleset is not null && AddedRuleset.Id == id ? AddedRuleset : null);

    public Task AddAsync(Ruleset aggregate, CancellationToken ct = default)
    {
        AddCalls++;
        AddedRuleset = aggregate;
        return Task.CompletedTask;
    }

    public void Remove(Ruleset aggregate) => RemoveCalls++;
}
