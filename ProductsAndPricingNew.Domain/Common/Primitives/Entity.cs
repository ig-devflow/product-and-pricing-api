namespace ProductsAndPricingNew.Domain.Common.Primitives;

public abstract class Entity<TId>
{
    public TId Id { get; protected set; } = default!;
}