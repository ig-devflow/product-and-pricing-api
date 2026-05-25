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
    public string Code { get; private set; } = null!;
    public int MinimumStayInWeeks { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Open;
    public TelephoneNumber? Telephone { get; private set; } = TelephoneNumber.Empty;
    public TelephoneNumber? EmergencyTelephone { get; private set; } = TelephoneNumber.Empty;
    public Address ContactAddress { get; private set; } = Address.Empty;
    public FinanceCode FinanceCode { get; private set; } = FinanceCode.Empty;
    public bool LmsAccess { get; private set; }
    public bool IsActive { get; private set; }
    public DateOnly? DecommissionDate { get; private set; }

    private School() { }

    private School(int centreId, string name, string code)
    {
        CentreId = centreId;
        Name = name;
        Code = code;
    }

    public void Rename(string name)
        => Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithCode(string legacyCode)
        => Code = legacyCode.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);

    public void WithMinimumStayInWeeks(int weeks)
    {
        EnsureValidMinimumStayInWeeks(weeks);
        MinimumStayInWeeks = weeks;
    }

    public void WithAgeRange(int? ageFrom, int? ageTo)
        => AgeRange = AgeRange.Create(ageFrom, ageTo);

    public void WithTelephone(string? value)
        => Telephone = TelephoneNumber.Create(value);

    public void WithEmergencyTelephone(string? value)
        => EmergencyTelephone = TelephoneNumber.Create(value);

    public void WithContactAddress(AddressDefinition? definition) =>
        ContactAddress = Address.Create(definition);

    public void WithFinanceCode(string? value)
        => FinanceCode = FinanceCode.Create(value);

    public void SetLmsAccess(bool value)
        => LmsAccess = value;

    public void SetIsActive(bool isActive)
    {
        var today = DateOnly.FromDateTime(DateTime.Now);

        if (DecommissionDate <= today)
            throw new DomainException("Cannot activate/deactivate decommissioned school.");

        IsActive = isActive;
    }

    public void WithDecommissionDate(DateOnly? date)
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
        private AgeRange _ageRange = AgeRange.Open;
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
            _legacyCode = legacyCode.AsRequiredDomainText(nameof(Code), Rules.CodeMaxLength);
        }

        public Builder WithMinimumStayInWeeks(int value)
        {
            EnsureValidMinimumStayInWeeks(value);

            _minimumStayInWeeks = value;
            return this;
        }

        public Builder WithAgeRange(int? ageFrom, int? ageTo)
        {
            _ageRange = AgeRange.Create(ageFrom, ageTo);
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

        public Builder WithContactAddress(AddressDefinition definition)
        {
            _contactAddress = Address.Create(definition);
            return this;
        }

        public Builder WithFinanceCode(string? code)
        {
            _financeCode = FinanceCode.Create(code);
            return this;
        }

        public Builder SetLmsActive(bool value)
        {
            _lmsAccess = value;
            return this;
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithDecommissionDate(DateOnly? value)
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
        public const int CodeMaxLength = 50;
    }
}