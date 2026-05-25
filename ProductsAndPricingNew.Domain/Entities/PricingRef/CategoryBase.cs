using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public abstract class CategoryBase : AggregateRoot<int>
{
    public int DivisionId { get; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }

    protected CategoryBase() { }

    protected CategoryBase(int divisionId, string name, bool isActive)
    {
        Guard.PositiveId(divisionId, nameof(DivisionId));

        DivisionId = divisionId;
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        IsActive = isActive;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
    
    public void SetIsActive(bool active) =>
        IsActive = active;

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}