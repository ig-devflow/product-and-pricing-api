using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Centre : AggregateRoot<int>
{
    private readonly List<CentreContact> _contacts = new();
    private readonly List<CentreTextContent> _texts = new();

    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public int CountryId { get; private set; }
    public int CurrencyId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsPhysicalCentre { get; private set; }
    public ContentFormat ContentFormat { get; private set; }
    public EmailAddress GeneralEmail { get; private set; } = EmailAddress.Empty;
    public EmailAddress AccommodationEmail { get; private set; } = EmailAddress.Empty;
    public TelephoneNumber Telephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber EmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber TransferEmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public HexColor BrandColor { get; private set; } = HexColor.Empty;
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile LogoImage { get; private set; } = ImageFile.Empty;

    public string? SchoolSponsorshipNumber { get; private set; }
    public string? VatNumber { get; private set; }
    public string? RegistrationNumber { get; private set; }
    public string? VatExemptionNumber { get; private set; }
    public string? ChequePayableTo { get; private set; }

    public decimal? Guarantees { get; private set; }
    public decimal? IndividualsRatio { get; private set; }
    public decimal? StaffingRatio { get; private set; }
    public decimal? EmptyBeds { get; private set; }

    public CentreBankDetails BankDetails { get; private set; } = CentreBankDetails.Empty;

    public IReadOnlyCollection<CentreContact> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<CentreTextContent> Texts => _texts.AsReadOnly();

    private Centre() { }

    public Centre(int divisionId, string name, string code, int countryId, int currencyId, ContentFormat printFormat)
    {
        Rename(name);
        ChangeCode(code);
        ChangeCountry(countryId);
        ChangeCurrency(currencyId);
        ChangePrintFormat(printFormat);

        IsActive = true;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeCountry(int countryId)
    {
        if (countryId <= 0)
            throw new DomainException("CountryId must be greater than zero.");

        CountryId = countryId;
    }

    public void ChangeCurrency(int currencyId)
    {
        if (currencyId <= 0)
            throw new DomainException("CurrencyId must be greater than zero.");

        CurrencyId = currencyId;
    }

    public void ChangeCode(string code)
        => Code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ChangePhysicalCentre(bool value)
        => IsPhysicalCentre = value;

    public void ChangePrintFormat(ContentFormat printFormat)
    {
        if (!Enum.IsDefined(printFormat) || printFormat == ContentFormat.None)
            throw new DomainException("Print format must be provided.");

        ContentFormat = printFormat;
    }

    public void ChangeGeneralEmail(string? value)
        => GeneralEmail = EmailAddress.Create(value);

    public void ChangeAccommodationEmail(string? value)
        => AccommodationEmail = EmailAddress.Create(value);

    public void ChangeTelephone(string? value)
        => Telephone = TelephoneNumber.Create(value);

    public void ChangeEmergencyTelephone(string? value)
        => EmergencyTelephone = TelephoneNumber.Create(value);

    public void ChangeTransferEmergencyTelephone(string? value)
        => TransferEmergencyTelephone = TelephoneNumber.Create(value);

    public void ChangeBrandColor(string? value)
        => BrandColor = HexColor.Create(value);

    public void ChangeContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        => ContactAddress = Address.Create(countryId, street, district, city, postalCode);

    public void ChangeLogo(byte[]? data, string? contentType, string? fileName)
        => LogoImage = ImageFile.Create(data, contentType, fileName, Rules.LogoMaxBytes);

    public void RemoveLogo()
        => LogoImage = ImageFile.Empty;

    public void AddOrUpdateContact(CentreContactType type, string name, string? email)
    {
        if (type == CentreContactType.None)
            throw new DomainException("Contact type must be provided.");

        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == type && !x.IsDeleted);

        if (existing is not null)
        {
            existing.Change(name, email);
            return;
        }

        _contacts.Add(CentreContact.Create(type, name, email));
    }

    public void RemoveContact(CentreContactType type)
    {
        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == type && !x.IsDeleted);
        existing?.Delete();
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int CodeMaxLength = 20;
        public const int LegalTextMaxLength = 100;
        public const int LogoMaxBytes = 5 * 1024 * 1024;
    }
}