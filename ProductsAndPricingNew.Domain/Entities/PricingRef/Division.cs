using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.ReferenceData;
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
    public string WebsiteUrl { get; private set; } = null!;
    public string? HeadOfficeEmail { get; private set; }
    public string? HeadOfficeTelephoneNo { get; private set; }
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile AccreditationBanner { get; private set; } = ImageFile.Empty;
    public IReadOnlyCollection<DivisionTextContent> Texts => _texts.AsReadOnly();

    private Division()
    {
    }

    private Division(string name, string websiteUrl)
    {
        Name = name;
        WebsiteUrl = websiteUrl;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeWebsite(string website)
        => WebsiteUrl = NormalizeWebsiteUrl(website);

    public void ChangeActiveState(bool isActive) => IsActive = isActive;

    public void ChangeTermsAndConditions(string? termsAndConditions)
        => TermsAndConditions = termsAndConditions.AsOptionalDomainText(nameof(TermsAndConditions), Rules.TermsAndConditionsMaxLength);

    public void ChangeGroupsPaymentTerms(string? groupsPaymentTerms)
        => GroupsPaymentTerms = groupsPaymentTerms.AsOptionalDomainText(nameof(GroupsPaymentTerms), Rules.GroupsPaymentTermsMaxLength);

    public void ChangeHeadOfficeEmail(string? headOfficeEmail)
        => HeadOfficeEmail = headOfficeEmail.AsOptionalDomainText(nameof(HeadOfficeEmail), Rules.HeadOfficeEmailMaxLength);

    public void ChangeHeadOfficeTelephone(string? headOfficeTelephone)
        => HeadOfficeTelephoneNo = headOfficeTelephone.AsOptionalDomainText(nameof(HeadOfficeTelephoneNo), Rules.HeadOfficeTelephoneNoMaxLength);

    public void ChangeContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        => ContactAddress = Address.Create(countryId, street, district, city, postalCode);

    public void ChangeAccreditationBanner(byte[]? data, string? contentType, string? fileName)
        => AccreditationBanner = ImageFile.Create(data, contentType, fileName, Rules.AccreditationBannerMaxBytes);

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

    private static string NormalizeWebsiteUrl(string websiteUrl)
    {
        string normalized = websiteUrl.AsRequiredDomainText(nameof(WebsiteUrl), Rules.WebsiteUrlMaxLength);

        if (!Uri.TryCreate(normalized, UriKind.Absolute, out Uri? uri) ||
            uri.Scheme is not ("http" or "https") ||
            string.IsNullOrWhiteSpace(uri.Host))
        {
            throw new DomainException("WebsiteUrl must be a valid http or https URL.");
        }

        return normalized;
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
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _websiteUrl = NormalizeWebsiteUrl(websiteUrl);
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
            _headOfficeEmail = value.AsOptionalDomainText(nameof(HeadOfficeEmail), Rules.HeadOfficeEmailMaxLength);
            return this;
        }

        public Builder HeadOfficeTelephone(string? value)
        {
            _headOfficeTelephoneNo = value.AsOptionalDomainText(nameof(HeadOfficeTelephoneNo), Rules.HeadOfficeTelephoneNoMaxLength);
            return this;
        }

        public Builder ContactAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        {
            _contactAddress = Address.Create(countryId, street, district, city, postalCode);
            return this;
        }

        public Builder AccreditationBanner(byte[]? data, string? contentType, string? fileName)
        {
            _accreditationBanner = ImageFile.Create(data, contentType, fileName, Rules.AccreditationBannerMaxBytes);
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
        public const int WebsiteUrlMaxLength = 255;
        public const int HeadOfficeEmailMaxLength = 50;
        public const int HeadOfficeTelephoneNoMaxLength = 50;
        public const int HeadOfficeTelephoneMaxLength = 50;
        public const int TermsAndConditionsMaxLength = 4000;
        public const int GroupsPaymentTermsMaxLength = 4000;
        public const int AccreditationBannerMaxBytes = 5 * 1024 * 1024;
    }
}
