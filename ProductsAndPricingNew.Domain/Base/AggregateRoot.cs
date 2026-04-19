using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Domain.Base;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
{
    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents;
    public EditInfo EditInfo { get; private set; } = null!;
    public byte[] RowVersion { get; private set; } = [];
    protected void Raise(object @event) => _domainEvents.Add(@event);
    public void ClearDomainEvents() => _domainEvents.Clear();
}