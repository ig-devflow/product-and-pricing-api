using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

// Внутренняя Entity агрегата Centre.
// Natural key: ContactType (composite (CentreId, ContactType) на уровне Persistence).
// Все mutator-методы помечены internal — изменения только через корень.
public sealed class CentreContact
{
    public CentreContactType ContactType { get; private set; }
    public string Name { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = EmailAddress.Empty;
    public ImageFile SignatureImage { get; private set; } = ImageFile.Empty;

    private CentreContact() { }

    private CentreContact(
        CentreContactType contactType,
        string name,
        EmailAddress email,
        ImageFile signatureImage)
    {
        ContactType = contactType;
        Name = name;
        Email = email;
        SignatureImage = signatureImage;
    }

    internal static CentreContact Create(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        EnsureValidType(definition.ContactType);

        return new CentreContact(
            definition.ContactType,
            definition.Name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength),
            EmailAddress.Create(definition.Email),
            ImageFile.Create(definition.SignatureImage, Rules.SignatureMaxBytes));
    }

    internal void Change(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        EnsureValidType(definition.ContactType);

        if (definition.ContactType != ContactType)
            throw new DomainException("Contact type cannot be changed.");

        Name = definition.Name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        Email = EmailAddress.Create(definition.Email);
        SignatureImage = ImageFile.Create(definition.SignatureImage, Rules.SignatureMaxBytes);
    }

    private static void EnsureValidType(CentreContactType type)
    {
        if (type == CentreContactType.None)
            throw new DomainException("Contact type must be specified.");

        if (!Enum.IsDefined(type))
            throw new DomainException($"Unsupported contact type '{type}'.");
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int SignatureMaxBytes = 2 * 1024 * 1024;
    }
}