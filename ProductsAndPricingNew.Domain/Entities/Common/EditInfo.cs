namespace ProductsAndPricingNew.Domain.Entities.Common;

public sealed class EditInfo : IEquatable<EditInfo>
{
    private EditInfo() { }

    private EditInfo(
        int createdBy,
        DateTimeOffset createTimestamp,
        int updatedBy,
        DateTimeOffset updateTimestamp)
    {
        if (createdBy <= 0)
            throw new DomainException("CreatedBy must be greater than zero.");

        if (updatedBy <= 0)
            throw new DomainException("UpdatedBy must be greater than zero.");

        if (updateTimestamp < createTimestamp)
            throw new DomainException("UpdateTimestamp cannot be earlier than CreateTimestamp.");

        CreatedBy = createdBy;
        CreateTimestamp = createTimestamp;
        UpdatedBy = updatedBy;
        UpdateTimestamp = updateTimestamp;
    }

    public int CreatedBy { get; private set; }
    public DateTimeOffset CreateTimestamp { get; private set; }
    public int UpdatedBy { get; private set; }
    public DateTimeOffset UpdateTimestamp { get; private set; }

    public static EditInfo Create(int userId, DateTimeOffset now) =>
        new(userId, now, userId, now);

    public EditInfo Touch(int userId, DateTimeOffset now) =>
        new(CreatedBy, CreateTimestamp, userId, now);

    public bool Equals(EditInfo? other)
    {
        if (other is null)
            return false;

        return CreatedBy == other.CreatedBy
               && CreateTimestamp == other.CreateTimestamp
               && UpdatedBy == other.UpdatedBy
               && UpdateTimestamp == other.UpdateTimestamp;
    }

    public override bool Equals(object? obj) =>
        obj is EditInfo other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(CreatedBy, CreateTimestamp, UpdatedBy, UpdateTimestamp);
}