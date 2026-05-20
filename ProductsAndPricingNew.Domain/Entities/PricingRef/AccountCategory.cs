namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class AccountCategory : CategoryBase
{
    private AccountCategory() { }

    private AccountCategory(int divisionId, string name, bool isActive)
        : base(divisionId, name, isActive)
    {
    }

    public static AccountCategory Create(int divisionId, string name, bool isActive)
    {
        return new AccountCategory(divisionId, name, isActive);
    }
}