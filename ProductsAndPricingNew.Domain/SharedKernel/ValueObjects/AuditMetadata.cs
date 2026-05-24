using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct AuditMetadata
{
    public int CreatedById { get; }
    public DateTimeOffset CreatedAt { get; }
    public int UpdatedById { get; }
    public DateTimeOffset UpdatedAt { get; }

    private AuditMetadata(
        int createdById,
        DateTimeOffset createdAt,
        int updatedById,
        DateTimeOffset updatedAt)
    {
        if (createdById <= 0)
            throw new DomainException("CreatedById must be greater than zero.");

        if (updatedById <= 0)
            throw new DomainException("UpdatedById must be greater than zero.");

        if (updatedAt < createdAt)
            throw new DomainException("UpdatedAt cannot be earlier than CreatedAt.");

        CreatedById = createdById;
        CreatedAt = createdAt;
        UpdatedById = updatedById;
        UpdatedAt = updatedAt;
    }

    public static AuditMetadata Create(int actorId, DateTimeOffset timestamp) =>
        new(actorId, timestamp, actorId, timestamp);

    public AuditMetadata MarkUpdated(int actorId, DateTimeOffset timestamp) =>
        new(CreatedById, CreatedAt, actorId, timestamp);
}
