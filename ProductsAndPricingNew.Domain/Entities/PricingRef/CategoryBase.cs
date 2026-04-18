using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public abstract class CategoryBase : AggregateRoot<int>
{
    protected CategoryBase() { }

    protected CategoryBase(int id, int divisionId, string name)
    {
        Id = id;
        DivisionId = divisionId;
        IsActive = true;
        IsDeleted = false;

        Rename(name);
    }

    public int DivisionId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required");

        Name = name.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void Delete() => IsDeleted = true;
    public void Restore() => IsDeleted = false;
}