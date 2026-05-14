using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreContact : IEmptyValueObject
{
    public CentreContactType ContactType { get; private set; }
    public string? Name { get; private set; }
    public EmailAddress Email { get; private set; } = EmailAddress.Empty;
    public ImageFile SignatureImage { get; private set; } = ImageFile.Empty;
    //public bool IsDeleted { get; private set; }

    private CentreContact()
    {
    }

    private CentreContact(CentreContactType type, string? name, EmailAddress email)
    {
        ContactType = type;
        Name = name;
        Email = email;
    }

    internal static CentreContact Create(CentreContactType type, string? name, string? email)
    {
        if (type == CentreContactType.None)
            return Empty;

        string normalizedName = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        var emailAddress = EmailAddress.Create(email);

        return new CentreContact(type, normalizedName, emailAddress);
    }

    internal void Change(string name, string? email)
    {
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        Email = EmailAddress.Create(email);
    }

    //internal void Delete() => IsDeleted = true;

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }

    public static CentreContact Empty => new(CentreContactType.None, null, EmailAddress.Empty);

    public bool IsEmpty => ContactType == CentreContactType.None;
}