/*
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using ProductsAndPricingNew.Domain.Common.Exceptions;

namespace ProductsAndPricingNew.Domain.Common.Primitives;

/// <summary>
/// Base class for a closed, behaviour-bearing set of values (a "smart enum").
/// The values are declared once as <c>public static readonly</c> fields on the derived type;
/// there is no separate constants class. Ids are stable and match the database seed.
/// </summary>
public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>, IComparable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Lazy<IReadOnlyList<TEnum>> AllValues =
        new(DiscoverValues, LazyThreadSafetyMode.ExecutionAndPublication);

    private static readonly Lazy<IReadOnlyDictionary<int, TEnum>> ById =
        new(() => AllValues.Value.ToDictionary(x => x.Id), LazyThreadSafetyMode.ExecutionAndPublication);

    public int Id { get; }
    public string Name { get; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static IReadOnlyList<TEnum> All => AllValues.Value;

    public static TEnum FromId(int id) =>
        ById.Value.TryGetValue(id, out TEnum? value)
            ? value
            : throw new DomainException($"Unknown {typeof(TEnum).Name} id: {id}.");

    public static bool TryFromId(int id, [NotNullWhen(true)] out TEnum? value) =>
        ById.Value.TryGetValue(id, out value);

    public static bool IsDefined(int id) => ById.Value.ContainsKey(id);

    private static IReadOnlyList<TEnum> DiscoverValues()
    {
        IEnumerable<TEnum> values = typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => typeof(TEnum).IsAssignableFrom(f.FieldType))
            .Select(f => (TEnum)f.GetValue(null)!);

        return values.ToList();
    }

    public bool Equals(Enumeration<TEnum>? other) => other is not null && other.Id == Id;

    public override bool Equals(object? obj) => obj is Enumeration<TEnum> other && Equals(other);

    public override int GetHashCode() => Id;

    public int CompareTo(Enumeration<TEnum>? other) => other is null ? 1 : Id.CompareTo(other.Id);

    public override string ToString() => Name;

    public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right) =>
        left is null ? right is null : left.Equals(right);

    public static bool operator !=(Enumeration<TEnum>? left, Enumeration<TEnum>? right) => !(left == right);
}
*/
