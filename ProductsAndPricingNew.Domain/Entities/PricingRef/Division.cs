using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Division : AggregateRoot<int>
{
    private readonly List<DivisionTextContent> _texts = new();

    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string? TermsAndConditions { get; private set; }
    public string? GroupsPaymentTerms { get; private set; }
    public WebsiteUrl WebsiteUrl { get; private set; } = WebsiteUrl.Empty;
    public EmailAddress HeadOfficeEmail { get; private set; } = EmailAddress.Empty;
    public TelephoneNumber HeadOfficeTelephoneNo { get; private set; } = TelephoneNumber.Empty;
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile AccreditationBanner { get; private set; } = ImageFile.Empty;
    public IReadOnlyCollection<DivisionTextContent> Texts => _texts.AsReadOnly();

    private Division()
    {
    }

    private Division(string name, WebsiteUrl websiteUrl)
    {
        Name = name;
        WebsiteUrl = websiteUrl;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeWebsite(string website)
        => WebsiteUrl = WebsiteUrl.Create(website).EnsureNotEmpty(nameof(WebsiteUrl));

    public void ChangeActiveState(bool isActive) => IsActive = isActive;

    public void ChangeTermsAndConditions(string? termsAndConditions)
        => TermsAndConditions = termsAndConditions.AsOptionalDomainText(nameof(TermsAndConditions), Rules.TermsAndConditionsMaxLength);

    public void ChangeGroupsPaymentTerms(string? groupsPaymentTerms)
        => GroupsPaymentTerms = groupsPaymentTerms.AsOptionalDomainText(nameof(GroupsPaymentTerms), Rules.GroupsPaymentTermsMaxLength);

    public void ChangeHeadOfficeEmail(string? headOfficeEmail)
        => HeadOfficeEmail = EmailAddress.Create(headOfficeEmail);

    public void ChangeHeadOfficeTelephone(string? headOfficeTelephone)
        => HeadOfficeTelephoneNo = TelephoneNumber.Create(headOfficeTelephone);

    public void ChangeContactAddress(AddressDefinition? definition) =>
        ContactAddress = Address.Create(definition);

    public void ChangeAccreditationBanner(ImageFileDefinition? definition) =>
        AccreditationBanner = ImageFile.Create(definition, Rules.AccreditationBannerMaxBytes);

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

        foreach (DivisionTextContent existing in _texts.Where(x => !x.IsDeleted).ToList())
        {
            var existingKey = (existing.ContentTemplateId, existing.AudienceId);

            if (!incomingKeys.Contains(existingKey))
                existing.Delete();
        }

        EnsureNoDuplicateActiveTextKeys();
    }

    private void UpsertText(TextContentDefinition definition)
    {
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

    private void EnsureNoDuplicateActiveTextKeys()
    {
        var activeKeys = new HashSet<(int ContentTemplateId, int? AudienceId)>();

        foreach (DivisionTextContent text in _texts.Where(x => !x.IsDeleted))
        {
            if (!activeKeys.Add((text.ContentTemplateId, text.AudienceId)))
                throw new DomainException("Duplicate text content for the same template and audience.");
        }
    }

    public sealed class Builder
    {
        private readonly string _name;
        private readonly WebsiteUrl _websiteUrl;
        private readonly List<TextContentDefinition> _texts = new();

        private bool _isActive;
        private string? _termsAndConditions;
        private string? _groupsPaymentTerms;
        private EmailAddress _headOfficeEmail = EmailAddress.Empty;
        private TelephoneNumber _headOfficeTelephoneNo = TelephoneNumber.Empty;
        private Address _contactAddress = Address.Empty;
        private ImageFile _accreditationBanner = ImageFile.Empty;

        public Builder(string name, string websiteUrl)
        {
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _websiteUrl = WebsiteUrl.Create(websiteUrl).EnsureNotEmpty(nameof(WebsiteUrl));
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder TermsAndConditions(string? value)
        {
            _termsAndConditions = value.AsOptionalDomainText(nameof(TermsAndConditions), Rules.TermsAndConditionsMaxLength);
            return this;
        }

        public Builder GroupsPaymentTerms(string? value)
        {
            _groupsPaymentTerms = value.AsOptionalDomainText(nameof(GroupsPaymentTerms), Rules.GroupsPaymentTermsMaxLength);
            return this;
        }

        public Builder HeadOfficeEmail(string? value)
        {
            _headOfficeEmail = EmailAddress.Create(value);
            return this;
        }

        public Builder HeadOfficeTelephone(string? value)
        {
            _headOfficeTelephoneNo = TelephoneNumber.Create(value);
            return this;
        }

        public Builder ContactAddress(AddressDefinition? definition)
        {
            _contactAddress = Address.Create(definition);
            return this;
        }

        public Builder AccreditationBanner(ImageFileDefinition definition)
        {
            _accreditationBanner = ImageFile.Create(definition, Rules.AccreditationBannerMaxBytes);
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

            division.ReplaceTexts(_texts);
            return division;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int TermsAndConditionsMaxLength = 4000;
        public const int GroupsPaymentTermsMaxLength = 4000;
        public const int AccreditationBannerMaxBytes = 5 * 1024 * 1024;
    }
}
