using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
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
    public CentreBankDetails BankDetails { get; private set; } = null!;
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

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeCode(string code) =>
        Code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);

    public void ChangeCurrency(int currencyId)
    {
        EnsureValidCurrency(currencyId);
        CurrencyId = currencyId;
    }

    public void ChangePrintFormat(PrintFormat printFormat)
    {
        EnsureValidPrintFormat(printFormat);
        PrintFormat = printFormat;
    }

    public void ChangeActive(bool isActive) => IsActive = isActive;

    public void ChangePhysicalCentre(bool value) => IsPhysicalCentre = value;

    public void ChangeGeneralEmail(string? value) =>
        GeneralEmail = EmailAddress.Create(value);

    public void ChangeAccommodationEmail(string? value) =>
        AccommodationEmail = EmailAddress.Create(value);

    public void ChangeTelephone(string? value) =>
        Telephone = TelephoneNumber.Create(value);

    public void ChangeEmergencyTelephone(string? value) =>
        EmergencyTelephone = TelephoneNumber.Create(value);

    public void ChangeTransferEmergencyTelephone(string? value) =>
        TransferEmergencyTelephone = TelephoneNumber.Create(value);

    public void ChangeBrandColor(string? value) =>
        BrandColor = HexColor.Create(value);

    public void ChangeContactAddress(AddressDefinition? definition) =>
        ContactAddress = Address.Create(definition);

    public void ChangeLogo(ImageFileDefinition? definition) =>
        LogoImage = ImageFile.Create(definition, Rules.LogoMaxBytes);

    public void ChangeSchoolSponsorshipNumber(string? value) =>
        SchoolSponsorshipNumber = value.AsOptionalDomainText(nameof(SchoolSponsorshipNumber), Rules.LegalTextMaxLength);

    public void ChangeVatNumber(string? value) =>
        VatNumber = value.AsOptionalDomainText(nameof(VatNumber), Rules.LegalTextMaxLength);

    public void ChangeRegistrationNumber(string? value) =>
        RegistrationNumber = value.AsOptionalDomainText(nameof(RegistrationNumber), Rules.LegalTextMaxLength);

    public void ChangeVatExemptionNumber(string? value) =>
        VatExemptionNumber = value.AsOptionalDomainText(nameof(VatExemptionNumber), Rules.LegalTextMaxLength);

    public void ChangeChequePayableTo(string? value) =>
        ChequePayableTo = value.AsOptionalDomainText(nameof(ChequePayableTo), Rules.LegalTextMaxLength);

    public void ChangeGuarantees(decimal? value) => Guarantees = value;

    public void ChangeIndividualsRatio(decimal? value) => IndividualsRatio = value;

    public void ChangeStaffingRatio(decimal? value) => StaffingRatio = value;

    public void ChangeEmptyBeds(decimal? value) => EmptyBeds = value;

    public void ChangeBankDetails(CentreBankDetailsDefinition? definition) =>
        BankDetails = CentreBankDetails.Create(definition);
    
    public void ReplaceContacts(IEnumerable<CentreContactDefinition> contacts)
    {
        ArgumentNullException.ThrowIfNull(contacts);

        var incomingTypes = new HashSet<CentreContactType>();
        foreach (CentreContactDefinition definition in contacts)
        {
            if (!incomingTypes.Add(definition.ContactType))
                throw new DomainException($"Duplicate contact for type '{definition.ContactType}'.");

            UpsertContact(definition);
        }

        foreach (CentreContact existing in _contacts.Where(x => !incomingTypes.Contains(x.ContactType)).ToList())
        {
            _contacts.Remove(existing);
        }
    }
    
    private void UpsertContact(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactType == definition.ContactType);
        if (existing is not null)
        {
            existing.Change(definition);
            return;
        }

        _contacts.Add(CentreContact.Create(definition));
    }

    public void ReplaceTexts(IEnumerable<TextContentDefinition> texts)
    {
        ArgumentNullException.ThrowIfNull(texts);

        var incomingKeys = new HashSet<(int ContentTemplateId, int? AudienceId)>();
        foreach (TextContentDefinition text in texts)
        {
            text.EnsureValid();

            if (!incomingKeys.Add(text.Key))
                throw new DomainException("Duplicate text content for the same template and audience.");

            UpsertText(text);
        }

        foreach (CentreTextContent existing in _texts.Where(x => !x.IsDeleted).ToList())
        {
            var existingKey = (existing.ContentTemplateId, existing.AudienceId);
            if (!incomingKeys.Contains(existingKey))
                existing.Delete();
        }

        EnsureNoDuplicateActiveTextKeys();
    }

    private void UpsertText(TextContentDefinition definition)
    {
        CentreTextContent? existing = _texts.FirstOrDefault(
            x => x.Matches(definition.ContentTemplateId, definition.NormalizedAudienceId));

        if (definition.IsEmpty)
        {
            existing?.Delete();
            return;
        }

        FormattedText text = FormattedText.Create(definition.Content, definition.Format);

        if (existing is null)
        {
            _texts.Add(new CentreTextContent(definition.ContentTemplateId, definition.NormalizedAudienceId, text));
            return;
        }

        existing.ChangeText(text);
    }

    private void EnsureNoDuplicateActiveTextKeys()
    {
        var activeKeys = new HashSet<(int ContentTemplateId, int? AudienceId)>();
        foreach (CentreTextContent text in _texts.Where(x => !x.IsDeleted))
        {
            if (!activeKeys.Add((text.ContentTemplateId, text.AudienceId)))
                throw new DomainException("Duplicate text content for the same template and audience.");
        }
    }

    private static void EnsureValidCurrency(int currencyId)
    {
        if (currencyId <= 0)
            throw new DomainException("CurrencyId must be greater than zero.");
    }

    private static void EnsureValidPrintFormat(PrintFormat printFormat)
    {
        if (!Enum.IsDefined(printFormat) || printFormat == PrintFormat.None)
            throw new DomainException("PrintFormat must be a valid value.");
    }

    public sealed class Builder
    {
        private readonly string _name;
        private readonly string _code;
        private readonly int _currencyId;
        private readonly PrintFormat _printFormat;

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
        private CentreBankDetailsDefinition? _bankDetails;
        private readonly List<CentreContactDefinition> _contacts = new();
        private readonly List<TextContentDefinition> _texts = new();

        public Builder(string name, string code, int currencyId, PrintFormat printFormat)
        {
            EnsureValidCurrency(currencyId);
            EnsureValidPrintFormat(printFormat);

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

        public Builder ContactAddress(AddressDefinition definition)
        {
            _contactAddress = Address.Create(definition);
            return this;
        }

        public Builder LogoImage(ImageFileDefinition definition)
        {
            _logoImage = ImageFile.Create(definition, Rules.LogoMaxBytes);
            return this;
        }

        public Builder SchoolSponsorshipNumber(string? value)
        {
            _schoolSponsorshipNumber = value.AsOptionalDomainText(nameof(SchoolSponsorshipNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder VatNumber(string? value)
        {
            _vatNumber = value.AsOptionalDomainText(nameof(VatNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder RegistrationNumber(string? value)
        {
            _registrationNumber = value.AsOptionalDomainText(nameof(RegistrationNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder VatExemptionNumber(string? value)
        {
            _vatExemptionNumber = value.AsOptionalDomainText(nameof(VatExemptionNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder ChequePayableTo(string? value)
        {
            _chequePayableTo = value.AsOptionalDomainText(nameof(ChequePayableTo), Rules.LegalTextMaxLength);
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

        public Builder BankDetails(CentreBankDetailsDefinition definition)
        {
            _bankDetails = definition;
            return this;
        }

        public Builder Contacts(IEnumerable<CentreContactDefinition> definitions)
        {
            ArgumentNullException.ThrowIfNull(definitions);
            _contacts.AddRange(definitions);
            return this;
        }

        public Builder Texts(IEnumerable<TextContentDefinition> definitions)
        {
            ArgumentNullException.ThrowIfNull(definitions);
            _texts.AddRange(definitions);
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

            centre.ChangeBankDetails(_bankDetails);
            centre.ReplaceContacts(_contacts);
            centre.ReplaceTexts(_texts);

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