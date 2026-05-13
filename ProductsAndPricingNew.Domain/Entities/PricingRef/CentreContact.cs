using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreContact : Entity<int>
{
    public CentreContactType ContactType { get; private set; }
    public string Name { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = EmailAddress.Empty;
    public ImageFile SignatureImage { get; private set; } = ImageFile.Empty;
    public bool IsDeleted { get; private set; }

    private CentreContact() { }

    private CentreContact(CentreContactType type, string name, string? email)
    {
        ContactType = type;
        Change(name, email);
    }

    internal static CentreContact Create(CentreContactType type, string name, string? email)
        => new(type, name, email);

    internal void Change(string name, string? email)
    {
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        Email = EmailAddress.Create(email);
    }

    internal void Delete() => IsDeleted = true;

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}