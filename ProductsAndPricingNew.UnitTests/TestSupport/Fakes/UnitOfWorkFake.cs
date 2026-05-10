using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

internal sealed class UnitOfWorkFake : IUnitOfWork
{
    public int SaveChangesCalls { get; private set; }
    public int SaveChangesResult { get; set; } = 1;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SaveChangesCalls++;
        return Task.FromResult(SaveChangesResult);
    }
}
