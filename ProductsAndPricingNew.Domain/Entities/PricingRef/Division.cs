using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;
using ProductsAndPricingNew.Domain.Entities.Common;
using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Division : AggregateRoot<int>
{
    private readonly List<DivisionTextContent> _texts = new();

    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string? TermsAndConditions { get; private set; }
    public string? GroupsPaymentTerms { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? HeadOfficeEmail { get; private set; }
    public string? HeadOfficeTelephoneNo { get; private set; }
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile AccreditationBanner { get; private set; } = ImageFile.Empty;
    public IReadOnlyCollection<DivisionTextContent> Texts => _texts;

    private Division() { }

    private Division(string name, string websiteUrl)
    {
        Rename(name);
        ChangeWebsite(websiteUrl);
    }

    public void Rename(string name) => Name = name.AsRequiredDomainText();
    public void ChangeWebsite(string website) => WebsiteUrl = website.AsRequiredDomainText();
    public void ChangeActiveState(bool isActive) => IsActive = isActive;
    public void ChangeTermsAndConditions(string? termsAndConditions) => TermsAndConditions = termsAndConditions.AsOptionalDomainText();
    public void ChangeGroupsPaymentTerms(string? groupsPaymentTerms) => GroupsPaymentTerms = groupsPaymentTerms.AsOptionalDomainText();
    public void ChangeHeadOfficeEmail(string? headOfficeEmail) => HeadOfficeEmail = headOfficeEmail.AsOptionalDomainText();
    public void ChangeHeadOfficeTelephone(string? headOfficeTelephone) => HeadOfficeTelephoneNo = headOfficeTelephone.AsOptionalDomainText();

    public void ChangeContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        => ContactAddress = Address.Create(countryId, street, district, city, postalCode);

    public void ChangeAccreditationBanner(byte[]? data, string? contentType, string? fileName)
        => AccreditationBanner = ImageFile.Create(data, contentType, fileName);

    public void ChangeTexts(IEnumerable<TextContentDefinition> texts)
    {
        ArgumentNullException.ThrowIfNull(texts);
        var incomingKeys = new HashSet<(int ContentTemplateId, int? AudienceId)>();

        foreach (TextContentDefinition text in texts)
        {
            if (!incomingKeys.Add(text.Key))
                throw new DomainException("Duplicate text content for the same template and audience.");

            UpsertText(text);
        }

        foreach (DivisionTextContent existing in _texts.Where(x => !x.IsDeleted).ToList())
        {
            var existingKey = (existing.ContentTemplateId, existing.AudienceId);

            if (!incomingKeys.Contains(existingKey))
                existing.Delete();
        }
    }

    private void UpsertText(TextContentDefinition definition)
    {
        definition.EnsureValidKey();
        DivisionTextContent? existing = _texts.FirstOrDefault(x => x.Matches(definition.ContentTemplateId, definition.NormalizedAudienceId));

        if (definition.IsEmpty)
        {
            existing?.Delete();
            return;
        }

        var text = FormattedText.Create(definition.Content, definition.Format);

        if (existing is null)
        {
            _texts.Add(DivisionTextContent.Create(definition, text));
            return;
        }

        existing.ChangeText(text);
    }

    public sealed class Builder
    {
        private readonly string _name;
        private readonly string _websiteUrl;
        private readonly List<TextContentDefinition> _texts = new();

        private bool _isActive;
        private string? _termsAndConditions;
        private string? _groupsPaymentTerms;
        private string? _headOfficeEmail;
        private string? _headOfficeTelephoneNo;
        private Address _contactAddress = Address.Empty;
        private ImageFile _accreditationBanner = ImageFile.Empty;

        public Builder(string name, string websiteUrl)
        {
            _name = name.AsRequiredDomainText();
            _websiteUrl = websiteUrl.AsRequiredDomainText();
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder TermsAndConditions(string? value)
        {
            _termsAndConditions = value.AsOptionalDomainText();
            return this;
        }

        public Builder GroupsPaymentTerms(string? value)
        {
            _groupsPaymentTerms = value.AsOptionalDomainText();
            return this;
        }

        public Builder HeadOfficeEmail(string? value)
        {
            _headOfficeEmail = value.AsOptionalDomainText();
            return this;
        }

        public Builder HeadOfficeTelephone(string? value)
        {
            _headOfficeTelephoneNo = value.AsOptionalDomainText();
            return this;
        }

        public Builder ContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        {
            _contactAddress = Address.Create(countryId, street, district, city, postalCode);
            return this;
        }

        public Builder AccreditationBanner(byte[]? data, string? contentType, string? fileName)
        {
            _accreditationBanner = ImageFile.Create(data, contentType, fileName);
            return this;
        }

        public Builder Texts(IEnumerable<TextContentDefinition> texts)
        {
            _texts.AddRange(texts);
            return this;
        }

        public Division Build()
        {
            Division division = new(_name, _websiteUrl)
            {
                IsActive = _isActive,
                TermsAndConditions = _termsAndConditions,
                GroupsPaymentTerms = _groupsPaymentTerms,
                HeadOfficeEmail = _headOfficeEmail,
                HeadOfficeTelephoneNo = _headOfficeTelephoneNo,
                ContactAddress = _contactAddress,
                AccreditationBanner = _accreditationBanner
            };

            division.ChangeTexts(_texts);
            return division;
        }
    }
}