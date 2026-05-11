using ProductsAndPricingNew.Domain.Common.Primitives;

namespace ProductsAndPricingNew.Domain.Edit;

public sealed class Editor : Entity<int>
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }

    private Editor()
    {
    }

    public static class Rules
    {
        public const int UserNameMaxLength = 100;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
        public const int EmailMaxLength = 50;
    }
}