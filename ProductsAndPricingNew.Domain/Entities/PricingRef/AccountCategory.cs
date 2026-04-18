namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class AccountCategory : CategoryBase
{
    private AccountCategory() { }

    public AccountCategory(int id, int divisionId, string name)
        : base(id, divisionId, name)
    {
    }
}