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
        EnsureValidDivision(divisionId);

        DivisionId = divisionId;
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        IsActive = isActive;
    }

    public void Rename(string name) => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
    public void ChangeActive(bool active) => IsActive = active;

    private static void EnsureValidDivision(int divisionId)
    {
        if (divisionId <= 0)
            throw new DomainException("DivisionId must be greater than zero.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}