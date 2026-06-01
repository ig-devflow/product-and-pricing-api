using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Centre : AggregateRoot<int>
{
    private readonly List<CentreContact> _contacts = new();
    private readonly List<CentreTextContent> _texts = new();

    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public int CurrencyId { get; private set; }
    public int PrintFormatId { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsPhysicalCentre { get; private set; }
    public EmailAddress? GeneralEmail { get; private set; } = EmailAddress.Empty;
    public EmailAddress? AccommodationEmail { get; private set; } = EmailAddress.Empty;
    public TelephoneNumber? Telephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber? EmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber? TransferEmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public HexColor BrandColor { get; private set; } = HexColor.Default;
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile LogoImage { get; private set; } = ImageFile.Empty;
    public string? SchoolSponsorshipNumber { get; private set; }
    public string? VatNumber { get; private set; }
    public string? RegistrationNumber { get; private set; }
    public string? VatExemptionNumber { get; private set; }
    public string? ChequePayableTo { get; private set; }
    public Percentage? Guarantees { get; private set; } = Percentage.Zero;
    public Percentage? IndividualsRatio { get; private set; } = Percentage.Zero;
    public Percentage? StaffingRatio { get; private set; } = Percentage.Zero;
    public Percentage? EmptyBeds { get; private set; } = Percentage.Zero;
    public CentreBankDetails BankDetails { get; private set; } = null!;
    public IReadOnlyCollection<CentreContact> Contacts => _contacts.AsReadOnly();
    public IReadOnlyCollection<CentreTextContent> Texts => _texts.AsReadOnly();

    private Centre() { }

    private Centre(string name, string code, int currencyId, int printFormatId)
    {
        Name = name;
        Code = code;
        CurrencyId = currencyId;
        PrintFormatId = printFormatId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithCode(string code) =>
        Code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);

    public void WithCurrency(int currencyId) =>
        CurrencyId = Guard.PositiveId(currencyId, nameof(CurrencyId));

    public void WithPrintFormat(int printFormatId) =>
        PrintFormatId = Guard.PositiveId(printFormatId, nameof(PrintFormatId));

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void SetIsPhysicalCentre(bool value) =>
        IsPhysicalCentre = value;

    public void WithGeneralEmail(string? value) =>
        GeneralEmail = EmailAddress.Create(value);

    public void WithAccommodationEmail(string? value) =>
        AccommodationEmail = EmailAddress.Create(value);

    public void WithTelephone(string? value) =>
        Telephone = TelephoneNumber.Create(value);

    public void WithEmergencyTelephone(string? value) =>
        EmergencyTelephone = TelephoneNumber.Create(value);

    public void WithTransferEmergencyTelephone(string? value) =>
        TransferEmergencyTelephone = TelephoneNumber.Create(value);

    public void WithBrandColor(string? value) =>
        BrandColor = HexColor.Create(value);

    public void WithContactAddress(AddressDefinition? definition) => // todo: CountryId is obligatory field
        ContactAddress = Address.Create(definition);

    public void WithLogo(ImageFileDefinition? definition) =>
        LogoImage = ImageFile.Create(definition, Rules.LogoMaxBytes);

    public void WithSchoolSponsorshipNumber(string? value) =>
        SchoolSponsorshipNumber = value.AsOptionalDomainText(nameof(SchoolSponsorshipNumber), Rules.LegalTextMaxLength);

    public void WithVatNumber(string? value) =>
        VatNumber = value.AsOptionalDomainText(nameof(VatNumber), Rules.LegalTextMaxLength);

    public void WithRegistrationNumber(string? value) =>
        RegistrationNumber = value.AsOptionalDomainText(nameof(RegistrationNumber), Rules.LegalTextMaxLength);

    public void WithVatExemptionNumber(string? value) =>
        VatExemptionNumber = value.AsOptionalDomainText(nameof(VatExemptionNumber), Rules.LegalTextMaxLength);

    public void WithChequePayableTo(string? value) =>
        ChequePayableTo = value.AsOptionalDomainText(nameof(ChequePayableTo), Rules.LegalTextMaxLength);

    public void WithGuarantees(decimal? value) =>
        Guarantees = Percentage.Create(value);

    public void WithIndividualsRatio(decimal? value) =>
        IndividualsRatio = Percentage.Create(value);

    public void WithStaffingRatio(decimal? value) =>
        StaffingRatio = Percentage.Create(value);

    public void WithEmptyBeds(decimal? value) =>
        EmptyBeds = Percentage.Create(value);

    public void WithBankDetails(CentreBankDetailsDefinition? definition) =>
        BankDetails = CentreBankDetails.Create(definition);

    public void WithContacts(IEnumerable<CentreContactDefinition> contacts)
    {
        ArgumentNullException.ThrowIfNull(contacts);

        var incomingTypeIds = new HashSet<int>();
        foreach (CentreContactDefinition definition in contacts)
        {
            if (!incomingTypeIds.Add(definition.ContactTypeId))
                throw new DomainException($"Duplicate contact for type id '{definition.ContactTypeId}'.");

            UpsertContact(definition);
        }

        foreach (CentreContact existing in _contacts.Where(x => !incomingTypeIds.Contains(x.ContactTypeId)).ToList())
        {
            _contacts.Remove(existing);
        }
    }

    private void UpsertContact(CentreContactDefinition definition)
    {
        ArgumentNullException.ThrowIfNull(definition);

        CentreContact? existing = _contacts.FirstOrDefault(x => x.ContactTypeId == definition.ContactTypeId);
        if (existing is not null)
        {
            existing.Change(definition);
            return;
        }

        _contacts.Add(CentreContact.Create(definition));
    }

    public void WithTexts(IEnumerable<TextContentDefinition> texts)
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
        CentreTextContent? existing = _texts.FirstOrDefault(x => x.Matches(definition.ContentTemplateId, definition.NormalizedAudienceId));

        if (definition.IsEmpty)
        {
            existing?.Delete();
            return;
        }

        var text = FormattedText.Create(definition.Content, definition.Format);

        if (existing is null)
        {
            _texts.Add(CentreTextContent.Create(definition, text));
            return;
        }

        existing.WithText(text);
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

    public sealed class Builder
    {
        private readonly string _name;
        private readonly string _code;
        private readonly int _currencyId;
        private readonly int _printFormatId;

        private bool _isActive;
        private bool _isPhysicalCentre;
        private EmailAddress _generalEmail = EmailAddress.Empty;
        private EmailAddress _accommodationEmail = EmailAddress.Empty;
        private TelephoneNumber _telephone = TelephoneNumber.Empty;
        private TelephoneNumber _emergencyTelephone = TelephoneNumber.Empty;
        private TelephoneNumber _transferEmergencyTelephone = TelephoneNumber.Empty;
        private HexColor _brandColor = HexColor.Default;
        private Address _contactAddress = Address.Empty;
        private ImageFile _logoImage = ImageFile.Empty;
        private string? _schoolSponsorshipNumber;
        private string? _vatNumber;
        private string? _registrationNumber;
        private string? _vatExemptionNumber;
        private string? _chequePayableTo;
        private Percentage _guarantees = Percentage.Zero;
        private Percentage _individualsRatio = Percentage.Zero;
        private Percentage _staffingRatio = Percentage.Zero;
        private Percentage _emptyBeds = Percentage.Zero;
        private CentreBankDetailsDefinition? _bankDetails;
        private readonly List<CentreContactDefinition> _contacts = new();
        private readonly List<TextContentDefinition> _texts = new();

        public Builder(string name, string code, int currencyId, int printFormatId)
        {
            Guard.PositiveId(currencyId, nameof(CurrencyId));
            Guard.PositiveId(printFormatId, nameof(PrintFormatId));

            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _code = code.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);
            _currencyId = currencyId;
            _printFormatId = printFormatId;
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder SetIsPhysicalCentre(bool value)
        {
            _isPhysicalCentre = value;
            return this;
        }

        public Builder WithGeneralEmail(string? value)
        {
            _generalEmail = EmailAddress.Create(value);
            return this;
        }

        public Builder WithAccommodationEmail(string? value)
        {
            _accommodationEmail = EmailAddress.Create(value);
            return this;
        }

        public Builder WithTelephone(string? value)
        {
            _telephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder WithEmergencyTelephone(string? value)
        {
            _emergencyTelephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder WithTransferEmergencyTelephone(string? value)
        {
            _transferEmergencyTelephone = TelephoneNumber.Create(value);
            return this;
        }

        public Builder WithBrandColor(string? value)
        {
            _brandColor = HexColor.Create(value);
            return this;
        }

        public Builder WithContactAddress(AddressDefinition definition)
        {
            _contactAddress = Address.Create(definition);
            return this;
        }

        public Builder WithLogoImage(ImageFileDefinition definition)
        {
            _logoImage = ImageFile.Create(definition, Rules.LogoMaxBytes);
            return this;
        }

        public Builder WithSchoolSponsorshipNumber(string? value)
        {
            _schoolSponsorshipNumber = value.AsOptionalDomainText(nameof(WithSchoolSponsorshipNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder WithVatNumber(string? value)
        {
            _vatNumber = value.AsOptionalDomainText(nameof(WithVatNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder WithRegistrationNumber(string? value)
        {
            _registrationNumber = value.AsOptionalDomainText(nameof(WithRegistrationNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder WithVatExemptionNumber(string? value)
        {
            _vatExemptionNumber = value.AsOptionalDomainText(nameof(WithVatExemptionNumber), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder WithChequePayableTo(string? value)
        {
            _chequePayableTo = value.AsOptionalDomainText(nameof(WithChequePayableTo), Rules.LegalTextMaxLength);
            return this;
        }

        public Builder WithGuarantees(decimal? value)
        {
            _guarantees = Percentage.Create(value);
            return this;
        }

        public Builder WithIndividualsRatio(decimal? value)
        {
            _individualsRatio = Percentage.Create(value);
            return this;
        }

        public Builder WithStaffingRatio(decimal? value)
        {
            _staffingRatio = Percentage.Create(value);
            return this;
        }

        public Builder WithEmptyBeds(decimal? value)
        {
            _emptyBeds = Percentage.Create(value);
            return this;
        }

        public Builder WithBankDetails(CentreBankDetailsDefinition definition)
        {
            _bankDetails = definition;
            return this;
        }

        public Builder WithContacts(IEnumerable<CentreContactDefinition> definitions)
        {
            ArgumentNullException.ThrowIfNull(definitions);
            _contacts.AddRange(definitions);
            return this;
        }

        public Builder WithTexts(IEnumerable<TextContentDefinition> definitions)
        {
            ArgumentNullException.ThrowIfNull(definitions);
            _texts.AddRange(definitions);
            return this;
        }

        public Centre Build()
        {
            Centre centre = new(_name, _code, _currencyId, _printFormatId)
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

            centre.WithBankDetails(_bankDetails);
            centre.WithContacts(_contacts);
            centre.WithTexts(_texts);

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