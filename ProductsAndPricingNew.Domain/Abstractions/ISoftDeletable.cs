namespace ProductsAndPricingNew.Domain.Abstractions;

public interface ISoftDeletable
{
    bool IsDeleted { get; }
}