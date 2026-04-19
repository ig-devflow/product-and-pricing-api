using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Entities.Common;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Division : AggregateRoot<int>
{
    private Division() { }

    private Division(string name)
    {
        ShowInDropdown = true;
        IsActive = true;

        Rename(name);
    }

    public string Name { get; private set; } = null!;
    public bool ShowInDropdown { get; private set; }
    public bool IsActive { get; private set; }
    public string? TermsAndConditions { get; private set; }
    public string? GroupsPaymentTerms { get; private set; }
    public string? WebsiteUrl { get; private set; }
    public string? HeadOfficeEmail { get; private set; }
    public string? HeadOfficeTelephoneNo { get; private set; }
    public Address? ContactAddress { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Division name is required");

        Name = name.Trim();
    }

    public void SetDropdownVisibility(bool visible) => ShowInDropdown = visible;
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ChangeTermsAndConditions(string? value) => TermsAndConditions = Normalize(value);
    public void ChangeGroupsPaymentTerms(string? value) => GroupsPaymentTerms = Normalize(value);
    public void ChangeWebsite(string? value) => WebsiteUrl = Normalize(value);
    public void ChangeHeadOfficeEmail(string? value) => HeadOfficeEmail = Normalize(value);
    public void ChangeHeadOfficeTelephone(string? value) => HeadOfficeTelephoneNo = Normalize(value);

    public void ChangeContactAddress(Address? address)
    {
        ContactAddress = address is null || address.IsEmpty ? null : address;
    }

    public void ClearContactAddress() => ContactAddress = null;

    private static string? Normalize(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    public sealed class Builder
    {
        private readonly string _name;
        private bool _showInDropdown = true;
        private bool _isActive = true;
        private string? _termsAndConditions;
        private string? _groupsPaymentTerms;
        private string? _websiteUrl;
        private string? _headOfficeEmail;
        private string? _headOfficeTelephoneNo;
        private Address? _contactAddress;

        public Builder(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Division name is required");

            _name = name.Trim();
        }

        public Builder ShowInDropdown(bool value)
        {
            _showInDropdown = value;
            return this;
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

        public Builder Website(string? value)
        {
            _websiteUrl = value;
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

        public Builder Address(Address? address)
        {
            _contactAddress = address;
            return this;
        }

        public Division Build()
        {
            var division = new Division(_name);

            division.SetDropdownVisibility(_showInDropdown);

            if (_isActive)
                division.Activate();
            else
                division.Deactivate();

            division.ChangeTermsAndConditions(_termsAndConditions);
            division.ChangeGroupsPaymentTerms(_groupsPaymentTerms);
            division.ChangeWebsite(_websiteUrl);
            division.ChangeHeadOfficeEmail(_headOfficeEmail);
            division.ChangeHeadOfficeTelephone(_headOfficeTelephoneNo);
            division.ChangeContactAddress(_contactAddress);

            return division;
        }
    }
}