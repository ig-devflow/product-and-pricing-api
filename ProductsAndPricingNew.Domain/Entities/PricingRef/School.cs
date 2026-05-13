using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class School : AggregateRoot<int>
{
    public int CentreId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? LegacyCode { get; private set; }
    public int MinimumAge { get; private set; }
    public int MinimumStayInWeeks { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Empty;
    public TelephoneNumber TelephoneNo { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber EmergencyNo { get; private set; } = TelephoneNumber.Empty;
    public Address Address { get; private set; } = Address.Empty;
    public FinanceCode FinanceCode { get; private set; } = FinanceCode.Empty;
    public bool LmsAccess { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly? DecommissionDate { get; private set; }

    private School() { }

    public School(int centreId, string name)
    {
        ChangeCentre(centreId);
        Rename(name);

        IsActive = true;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeCentre(int centreId)
    {
        if (centreId <= 0)
            throw new DomainException("CentreId must be greater than zero.");

        CentreId = centreId;
    }

    public void ChangeMinimumAge(int age)
    {
        if (age < 0)
            throw new DomainException("Minimum age cannot be negative.");

        if (age > AgeRange.Rules.MaxAge)
            throw new DomainException($"Minimum age must not exceed {AgeRange.Rules.MaxAge}.");

        MinimumAge = age;
    }

    public void ChangeMinimumStayInWeeks(int weeks)
    {
        if (weeks < 0)
            throw new DomainException("Minimum stay cannot be negative.");

        MinimumStayInWeeks = weeks;
    }

    public void ChangeLegacyCode(string? legacyCode)
        => LegacyCode = legacyCode.AsOptionalDomainText(nameof(LegacyCode), Rules.LegacyCodeMaxLength);

    public void ChangeAgeRange(int? from, int? to)
        => AgeRange = AgeRange.Create(from, to);

    public void ChangeTelephoneNo(string? value)
        => TelephoneNo = TelephoneNumber.Create(value);

    public void ChangeEmergencyNo(string? value)
        => EmergencyNo = TelephoneNumber.Create(value);

    public void ChangeAddress(int? countryId, string? street, string? district, string? city, string? postalCode)
        => Address = Address.Create(countryId, street, district, city, postalCode);

    public void ChangeFinanceCode(string? value)
        => FinanceCode = FinanceCode.Create(value);

    public void ChangeLmsAccess(bool value)
        => LmsAccess = value;

    public void Activate()
    {
        if (DecommissionDate is not null)
            throw new DomainException("Cannot activate decommissioned school.");

        IsActive = true;
    }

    public void Deactivate()
        => IsActive = false;

    public void Decommission(DateOnly date)
    {
        DecommissionDate = date;
        IsActive = false;
    }

    public void ClearDecommissionDate()
        => DecommissionDate = null;

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int LegacyCodeMaxLength = 50;
    }
}