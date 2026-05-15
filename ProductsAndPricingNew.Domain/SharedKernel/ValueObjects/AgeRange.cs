using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly struct AgeRange : IEquatable<AgeRange>, IEmptyValueObject
{
    public int? From { get; }
    public int? To { get; }

    private AgeRange(int? from, int? to)
    {
        From = from;
        To = to;
    }

    public bool IsEmpty => From is null && To is null;
    public static AgeRange Empty { get; } = new(null, null);

    public static AgeRange Create(int? from, int? to)
    {
        if (from is null && to is null)
            return Empty;

        EnsureValid(from, to);

        return new AgeRange(from, to);
    }

    public static bool IsValid(int? from, int? to)
    {
        if (from is null && to is null)
            return true;

        if (from is < Rules.MinAge)
            return false;

        if (to is < Rules.MinAge)
            return false;

        if (from > Rules.MaxAge)
            return false;

        if (to > Rules.MaxAge)
            return false;

        return !from.HasValue || !to.HasValue || from <= to;
    }

    private static void EnsureValid(int? from, int? to)
    {
        if (from is < Rules.MinAge)
            throw new DomainException("Age from cannot be negative.");

        if (to is < Rules.MinAge)
            throw new DomainException("Age to cannot be negative.");

        if (from > Rules.MaxAge)
            throw new DomainException($"Age from must not exceed {Rules.MaxAge}.");

        if (to > Rules.MaxAge)
            throw new DomainException($"Age to must not exceed {Rules.MaxAge}.");

        if (from.HasValue && to.HasValue && from > to)
            throw new DomainException("Age from must be less than or equal to age to.");
    }

    public bool Equals(AgeRange other) => From == other.From && To == other.To;

    public override bool Equals(object? obj) => obj is AgeRange other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(From, To);

    public override string ToString() =>
        IsEmpty ? string.Empty : $"{From?.ToString() ?? "-"}-{To?.ToString() ?? "-"}";

    public static bool operator ==(AgeRange left, AgeRange right) => left.Equals(right);
    public static bool operator !=(AgeRange left, AgeRange right) => !left.Equals(right);

    public static class Rules
    {
        public const int MinAge = 0;
        public const int MaxAge = 120;
    }
}