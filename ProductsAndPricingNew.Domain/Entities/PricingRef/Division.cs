using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class Division : AggregateRoot<int>
{
    private Division() { }

    public Division(int id, string name)
    {
        Id = id;
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
}