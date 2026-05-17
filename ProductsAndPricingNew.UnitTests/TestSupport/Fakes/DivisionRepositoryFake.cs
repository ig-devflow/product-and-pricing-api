using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.Repositories;
using ProductsAndPricingNew.UnitTests.TestSupport.Builders;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class DivisionRepositoryFake : IDivisionRepository
{
    private readonly Dictionary<int, Division> _divisions = new();

    public int NextId { get; set; } = 1;
    public int AddCalls { get; private set; }
    public int RemoveCalls { get; private set; }
    public Division? AddedDivision { get; private set; }
    public Division? RemovedDivision { get; private set; }

    public DivisionRepositoryFake WithDivision(int id, Division division)
    {
        DivisionBuilder.SetId(division, id);
        _divisions[id] = division;
        return this;
    }

    public Task<Division?> GetByIdAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_divisions.GetValueOrDefault(id));

    public Task<Division?> GetByIdWithTextsAsync(int id, CancellationToken ct = default)
        => Task.FromResult(_divisions.GetValueOrDefault(id));

    public Task AddAsync(Division aggregate, CancellationToken ct = default)
    {
        AddCalls++;
        AddedDivision = aggregate;

        if (aggregate.Id <= 0)
            DivisionBuilder.SetId(aggregate, NextId++);

        _divisions[aggregate.Id] = aggregate;
        return Task.CompletedTask;
    }

    public void Remove(Division aggregate)
    {
        RemoveCalls++;
        RemovedDivision = aggregate;
        _divisions.Remove(aggregate.Id);
    }
}