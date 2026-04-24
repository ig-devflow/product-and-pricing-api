using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Division : AggregateRoot<int>
{
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string? TermsAndConditions { get; private set; }
    public string? GroupsPaymentTerms { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? HeadOfficeEmail { get; private set; }
    public string? HeadOfficeTelephoneNo { get; private set; }
    public Address ContactAddress { get; private set; } = Address.Empty;
    public ImageFile AccreditationBanner { get; private set; } = ImageFile.Empty;

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
    public void ChangeContactAddress(Address address) => ContactAddress = address;
    public void ChangeAccreditationBanner(ImageFile value) => AccreditationBanner = value;

    public sealed class Builder
    {
        private readonly string _name;
        private readonly string _websiteUrl;
        private bool _isActive;
        private string? _termsAndConditions;
        private string? _groupsPaymentTerms;
        private string? _headOfficeEmail;
        private string? _headOfficeTelephoneNo;
        private Address _contactAddress = Address.Empty;
        private ImageFile _accreditationBanner = ImageFile.Empty;

        public Builder(string name, string websiteUrl)
        {
            _name = name;
            _websiteUrl = websiteUrl;
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder TermsAndConditions(string? value)
        {
            _termsAndConditions = value;
            return this;
        }

        public Builder GroupsPaymentTerms(string? value)
        {
            _groupsPaymentTerms = value;
            return this;
        }

        public Builder HeadOfficeEmail(string? value)
        {
            _headOfficeEmail = value;
            return this;
        }

        public Builder HeadOfficeTelephone(string? value)
        {
            _headOfficeTelephoneNo = value;
            return this;
        }

        public Builder ContactAddress(Address address)
        {
            _contactAddress = address;
            return this;
        }

        public Builder AccreditationBanner(ImageFile accreditationBanner)
        {
            _accreditationBanner = accreditationBanner;
            return this;
        }

        public Division Build()
        {
            Division division = new(_name, _websiteUrl);

            division.ChangeActiveState(_isActive);
            division.ChangeTermsAndConditions(_termsAndConditions);
            division.ChangeGroupsPaymentTerms(_groupsPaymentTerms);
            division.ChangeHeadOfficeEmail(_headOfficeEmail);
            division.ChangeHeadOfficeTelephone(_headOfficeTelephoneNo);
            division.ChangeContactAddress(_contactAddress);
            division.ChangeAccreditationBanner(_accreditationBanner);

            return division;
        }
    }
}