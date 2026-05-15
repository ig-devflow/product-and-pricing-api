using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Edit;

public sealed class Editor : Entity<int>
{
    public string UserName { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public EmailAddress Email { get; init; }

    private Editor() { }
    
    private Editor(string userName, string firstName, string lastName, EmailAddress email)
    {
        UserName = userName;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static Editor Create(string userName, string firstName, string lastName, string email)
    {
        string normalizedUserName = userName.AsRequiredDomainText(nameof(userName), Rules.UserNameMaxLength);
        string normalizedFirstName = firstName.AsRequiredDomainText(nameof(firstName), Rules.FirstNameMaxLength);
        string normalizedLastName = lastName.AsRequiredDomainText(nameof(lastName), Rules.LastNameMaxLength);
        string normalizedEmail = email.AsRequiredDomainText(nameof(email));
        
        return new Editor(normalizedUserName, normalizedFirstName, normalizedLastName, EmailAddress.Create(normalizedEmail));
    }

    public static class Rules
    {
        public const int UserNameMaxLength = 100;
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
    }
}