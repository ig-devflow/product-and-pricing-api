using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class School : AggregateRoot<int>
{
    public int CentreId { get; private set; }
    public string Name { get; private set; } = null!;
    public string LegacyCode { get; private set; }
    public int MinimumStayInWeeks { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Empty;
    public TelephoneNumber Telephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber EmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public Address ContactAddress { get; private set; } = Address.Empty;
    public FinanceCode FinanceCode { get; private set; } = FinanceCode.Empty;
    public bool LmsAccess { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly? DecommissionDate { get; private set; }

    private School() { }

    private School(int centreId, string name, string legacyCode)
    {
        CentreId = centreId;
        Name = name;
        LegacyCode = legacyCode;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeLegacyCode(string legacyCode)
        => LegacyCode = legacyCode.AsRequiredDomainText(nameof(LegacyCode), Rules.LegacyCodeMaxLength);

    public void ChangeMinimumStayInWeeks(int weeks)
    {
        EnsureValidMinimumStayInWeeks(weeks);
        MinimumStayInWeeks = weeks;
    }

    public void ChangeAgeRange(int? from, int? to)
        => AgeRange = AgeRange.Create(from, to);

    public void ChangeTelephone(string? value)
        => Telephone = TelephoneNumber.Create(value);

    public void ChangeEmergencyTelephone(string? value)
        => EmergencyTelephone = TelephoneNumber.Create(value);

    public void ChangeContactAddress(AddressDefinition? definition) =>
        ContactAddress = Address.Create(definition);

    public void ChangeFinanceCode(string? value)
        => FinanceCode = FinanceCode.Create(value);

    public void ChangeLmsAccess(bool value)
        => LmsAccess = value;

    public void ChangeActive(bool isActive)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        if (DecommissionDate <= today)
            throw new DomainException("Cannot activate/deactivate decommissioned school.");

        IsActive = isActive;
    }

    public void ChangeDecommissionDate(DateOnly? date)
        => DecommissionDate = date;

    private static void EnsureValidCentre(int centreId)
    {
        if (centreId <= 0)
            throw new DomainException("CentreId must be greater than zero.");
    }

    private static void EnsureValidMinimumStayInWeeks(int minimumStayInWeeks)
    {
        if (minimumStayInWeeks <= 0)
            throw new DomainException("Minimum stay in weeks must be greater than zero.");
    }

    public sealed class Builder
    {
        private readonly int _centreId;
        private readonly string _name;
        private readonly string _legacyCode;

        private int _minimumStayInWeeks;
        private AgeRange _ageRange;
        private TelephoneNumber _telephone = TelephoneNumber.Empty;
        private TelephoneNumber _emergencyTelephone = TelephoneNumber.Empty;
        private Address _contactAddress = Address.Empty;
        private FinanceCode _financeCode = FinanceCode.Empty;
        private bool _lmsAccess;
        private bool _isActive;
        private DateOnly? _decommissionDate;

        public Builder(int centreId, string name, string legacyCode)
        {
            EnsureValidCentre(centreId);

            _centreId = centreId;
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _legacyCode = legacyCode.AsRequiredDomainText(nameof(LegacyCode), Rules.LegacyCodeMaxLength);
        }

        public Builder MinimumStayInWeeks(int value)
        {
            EnsureValidMinimumStayInWeeks(value);

            _minimumStayInWeeks = value;
            return this;
        }

        public Builder SetAgeRange(int? from, int? to)
        {
            _ageRange = AgeRange.Create(from, to);
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

        public Builder ContactAddress(AddressDefinition definition)
        {
            _contactAddress = Address.Create(definition);
            return this;
        }

        public Builder SetFinanceCode(string? code)
        {
            _financeCode = FinanceCode.Create(code);
            return this;
        }

        public Builder LmsActive(bool value)
        {
            _lmsAccess = value;
            return this;
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder DecommissionDate(DateOnly? value)
        {
            _decommissionDate = value;
            return this;
        }

        public School Build()
        {
            School school = new(_centreId, _name, _legacyCode)
            {
                MinimumStayInWeeks = _minimumStayInWeeks,
                AgeRange = _ageRange,
                Telephone = _telephone,
                EmergencyTelephone = _emergencyTelephone,
                ContactAddress = _contactAddress,
                FinanceCode = _financeCode,
                LmsAccess = _lmsAccess,
                IsActive = _isActive,
                DecommissionDate = _decommissionDate
            };

            return school;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
        public const int LegacyCodeMaxLength = 50;
    }
}