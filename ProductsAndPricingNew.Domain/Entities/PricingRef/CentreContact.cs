using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreContact
{
    public int ContactTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public EmailAddress? Email { get; private set; } = EmailAddress.Empty;
    public ImageFile SignatureImage { get; private set; } = ImageFile.Empty;

    private CentreContact() { }

    private CentreContact(
        int contactTypeId,
        string name,
        EmailAddress email,
        ImageFile signatureImage)
    {
        ContactTypeId = contactTypeId;
        Name = name;
        Email = email;
        SignatureImage = signatureImage;
    }

    internal static CentreContact Create(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);
        Guard.PositiveId(definition.ContactTypeId, nameof(ContactTypeId));

        return new CentreContact(
            definition.ContactTypeId,
            definition.Name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength),
            EmailAddress.Create(definition.Email),
            ImageFile.Create(definition.SignatureImage, Rules.SignatureMaxBytes));
    }

    internal void Change(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        if (definition.ContactTypeId != ContactTypeId)
            throw new DomainException("Contact type cannot be changed.");

        Name = definition.Name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
        Email = EmailAddress.Create(definition.Email);
        SignatureImage = ImageFile.Create(definition.SignatureImage, Rules.SignatureMaxBytes);
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int SignatureMaxBytes = 2 * 1024 * 1024;
    }
}