using ProductsAndPricingNew.Domain.Abstractions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Common.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot, IAuditable, ISoftDeletable
{
    private readonly IList<object> _domainEvents = new List<object>();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
    public AuditMetadata AuditMetadata { get; private set; }
    public bool IsDeleted { get; protected set; }
    public byte[] Version { get; private set; } = [];

    public bool HasVersion(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
            return false;

        Span<byte> expectedVersion = stackalloc byte[8];

        if (!Convert.TryFromBase64String(version, expectedVersion, out int bytesWritten))
            return false;

        if (bytesWritten != 8)
            return false;

        return Version.AsSpan().SequenceEqual(expectedVersion);
    }
    protected void Raise(object @event) => _domainEvents.Add(@event);
    //public void ClearDomainEvents() => _domainEvents.Clear();
}