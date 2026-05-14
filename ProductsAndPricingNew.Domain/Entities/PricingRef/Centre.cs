using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Centre : AggregateRoot<int>
{
    private readonly List<CentreContact> _contacts = new();
    private readonly List<CentreTextContent> _texts = new();

    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public int CurrencyId { get; private set; }
    public PrintFormat PrintFormat { get; private set; } = PrintFormat.None;
    public bool IsActive { get; private set; }
    public bool IsPhysicalCentre { get; private set; }
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

    private Centre(string name, string code, int currencyId, PrintFormat printFormat)
    {
        Name = name;
        Code = code;
        CurrencyId = currencyId;
        PrintFormat = printFormat;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeCurrency(int currencyId)
    {
        if (currencyId <= 0)
            throw new DomainException("CurrencyId must be greater than zero.");

        CurrencyId = currencyId;
    }

    public void ChangeCode(string code)
        => Code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);

    public void ChangeActive(bool isActive)
        => IsActive = isActive;

    public void ChangePhysicalCentre(bool value)
        => IsPhysicalCentre = value;

    public void ChangePrintFormat(PrintFormat printFormat)
    {
        if (!Enum.IsDefined(printFormat) || printFormat == PrintFormat.None)
            throw new DomainException("Print format must be provided.");

        PrintFormat = printFormat;
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

    public void ChangeContactAddress(int countryId, string? street, string? district, string? city, string? postalCode)
    {
        if (countryId <= 0)
            throw new DomainException("CountryId must be greater than zero.");

        ContactAddress = Address.Create(countryId, street, district, city, postalCode);
    }

    public void ChangeLogo(byte[]? data, string? contentType, string? fileName)
        => LogoImage = ImageFile.Create(data, contentType, fileName, Rules.LogoMaxBytes);

    public void UpsertContact(CentreContactType type, string name, string? email)
    {
        if (type == CentreContactType.None)
            _contacts.Clear();

        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == type);
        if (existing is not null)
        {
            existing.Change(name, email);
            return;
        }

        _contacts.Add(CentreContact.Create(type, name, email));
    }

    public void RemoveContact(CentreContactType type)
    {
        // CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == type && !x.IsDeleted);
        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == type);
        //existing?.Delete();
    }

    public sealed class Builder
    {
        private readonly string _name;
        private readonly string _code;
        private readonly int _currencyId;
        private readonly PrintFormat _printFormat;
        private readonly List<TextContentDefinition> _texts = new();

        private bool _isActive;
        private bool _isPhysicalCentre;
        private EmailAddress _generalEmail = EmailAddress.Empty;
        private EmailAddress _accommodationEmail = EmailAddress.Empty;
        private TelephoneNumber _telephone = TelephoneNumber.Empty;
        private TelephoneNumber _emergencyTelephone = TelephoneNumber.Empty;
        private TelephoneNumber _transferEmergencyTelephone = TelephoneNumber.Empty;
        private HexColor _brandColor = HexColor.Empty;
        private Address _contactAddress = Address.Empty;
        private ImageFile _logoImage = ImageFile.Empty;
        private string? _schoolSponsorshipNumber;
        private string? _vatNumber;
        private string? _registrationNumber;
        private string? _vatExemptionNumber;
        private string? _chequePayableTo;
        private decimal? _guarantees;
        private decimal? _individualsRatio;
        private decimal? _staffingRatio;
        private decimal? _emptyBeds;
        private List<CentreContact> _contacts = new();
        private CentreBankDetails _bankDetails = CentreBankDetails.Empty;

        public Builder(string name, string code, int currencyId, PrintFormat printFormat)
        {
            if (currencyId <= 0)
                throw new DomainException("CurrencyId must be greater than zero.");

            if (!Enum.IsDefined(typeof(PrintFormat), printFormat) || printFormat == PrintFormat.None)
                throw new DomainException("PrintFormat must be a valid enum.");

            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);
            _currencyId = currencyId;
            _printFormat = printFormat;
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder IsPhysicalCentre(bool value)
        {
            _isPhysicalCentre = value;
            return this;
        }

        public Builder GeneralEmail(string? value)
        {
            _generalEmail = EmailAddress.Create(value);
            return this;
        }

        public Builder AccommodationEmail(string? value)
        {
            _accommodationEmail = EmailAddress.Create(value);
            return this;
        }

        public Builder Telephone(string? value)
        {
            _telephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder EmergencyTelephone(string? value)
        {
            _emergencyTelephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder TransferEmergencyTelephone(string? value)
        {
            _transferEmergencyTelephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder BrandColor(string? value)
        {
            _brandColor = HexColor.Create(value);
            return this;
        }

        public Builder ContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        {
            if (!countryId.HasValue)
                throw new DomainException("Country ID must be provided.");

            _contactAddress = Address.Create(countryId, street, district, city, postalCode);
            return this;
        }

        public Builder LogoBanner(byte[]? data, string? contentType, string? fileName)
        {
            _logoImage = ImageFile.Create(data, contentType, fileName, Rules.LogoMaxBytes);
            return this;
        }

        public Builder Texts(IEnumerable<TextContentDefinition> texts)
        {
            _texts.AddRange(texts);
            return this;
        }

        public Builder SchoolSponsorshipNumber(string? value)
        {
            _schoolSponsorshipNumber = value.AsOptionalDomainText(nameof(SchoolSponsorshipNumber));
            return this;
        }

        public Builder VatNumber(string? value)
        {
            _vatNumber = value.AsOptionalDomainText(nameof(VatNumber));
            return this;
        }

        public Builder RegistrationNumber(string? value)
        {
            _registrationNumber = value.AsOptionalDomainText(nameof(RegistrationNumber));
            return this;
        }

        public Builder VatExemptionNumber(string? value)
        {
            _vatExemptionNumber = value.AsOptionalDomainText(nameof(VatExemptionNumber));
            return this;
        }

        public Builder ChequePayableTo(string? value)
        {
            _chequePayableTo = value.AsOptionalDomainText(nameof(ChequePayableTo));
            return this;
        }

        public Builder Guarantees(decimal? value)
        {
            _guarantees = value;
            return this;
        }

        public Builder IndividualsRatio(decimal? value)
        {
            _individualsRatio = value;
            return this;
        }

        public Builder StaffingRatio(decimal? value)
        {
            _staffingRatio = value;
            return this;
        }

        public Builder EmptyBeds(decimal? value)
        {
            _emptyBeds = value;
            return this;
        }

        public Centre Build()
        {
            Centre centre = new(_name, _code, _currencyId, _printFormat)
            {
                IsActive = _isActive,
                IsPhysicalCentre = _isPhysicalCentre,
                GeneralEmail = _generalEmail,
                AccommodationEmail = _accommodationEmail,
                Telephone = _telephone,
                EmergencyTelephone = _emergencyTelephone,
                TransferEmergencyTelephone = _transferEmergencyTelephone,
                BrandColor = _brandColor,
                ContactAddress = _contactAddress,
                LogoImage = _logoImage,
                SchoolSponsorshipNumber = _schoolSponsorshipNumber,
                VatNumber = _vatNumber,
                RegistrationNumber = _registrationNumber,
                VatExemptionNumber = _vatExemptionNumber,
                ChequePayableTo = _chequePayableTo,
                Guarantees = _guarantees,
                IndividualsRatio = _individualsRatio,
                StaffingRatio = _staffingRatio,
                EmptyBeds = _emptyBeds,
            };

            //centre.ReplaceTexts(_texts);
            return centre;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int CodeMaxLength = 6;
        public const int LegalTextMaxLength = 100;
        public const int LogoMaxBytes = 5 * 1024 * 1024;
    }
}