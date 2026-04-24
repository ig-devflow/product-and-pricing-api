using ProductsAndPricingNew.Domain.Common.Errors;

namespace ProductsAndPricingNew.Domain.Entities.Common;

public readonly struct AuditMetadata : IEquatable<AuditMetadata>
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

    public bool Equals(AuditMetadata other) =>
        CreatedById == other.CreatedById
        && CreatedAt == other.CreatedAt
        && UpdatedById == other.UpdatedById
        && UpdatedAt == other.UpdatedAt;

    public override bool Equals(object? obj) =>
        obj is AuditMetadata other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(CreatedById, CreatedAt, UpdatedById, UpdatedAt);

    public static bool operator ==(AuditMetadata left, AuditMetadata right) => left.Equals(right);
    public static bool operator !=(AuditMetadata left, AuditMetadata right) => !left.Equals(right);
}