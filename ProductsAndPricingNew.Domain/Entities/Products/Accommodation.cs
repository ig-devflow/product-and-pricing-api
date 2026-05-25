using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Accommodation : AggregateRoot<int>
{
    public string Name { get; private set; } = null!;
    public int AccommodationTypeId { get; private set; }
    public bool IsActive { get; private set; }
    public int MinimumStayInWeeks { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Open;
    public bool IsCommitted { get; private set; }
    public bool IsNonCommitted { get; private set; }

    private Accommodation() { }

    private Accommodation(string name, int accommodationTypeId)
    {
        Name = name;
        AccommodationTypeId = accommodationTypeId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithAccommodationType(int accommodationTypeId) =>
        AccommodationTypeId = Guard.PositiveId(accommodationTypeId, nameof(AccommodationTypeId));

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void WithMinimumStay(int weeks)
    {
        if (weeks < 0)
            throw new DomainException("Minimum stay in weeks cannot be negative.");

        MinimumStayInWeeks = weeks;
    }

    public void WithAgeRange(int? ageFrom, int? ageTo) =>
        AgeRange = AgeRange.Create(ageFrom, ageTo);

    public void SetCommitment(bool isCommitted) =>
        IsCommitted = isCommitted;
    
    public void SetNonCommitment(bool isNonCommitted) =>
        IsNonCommitted = isNonCommitted;

    public sealed class Builder
    {
        private readonly string _name;
        private readonly int _accommodationTypeId;

        private bool _isActive = true;
        private int _minimumStayInWeeks;
        private AgeRange _ageRange = AgeRange.Open;
        private bool _isCommitted;
        private bool _isNonCommitted;

        public Builder(string name, int accommodationTypeId)
        {
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _accommodationTypeId = Guard.PositiveId(accommodationTypeId, nameof(AccommodationTypeId));
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithMinimumStayInWeeks(int weeks)
        {
            if (weeks < 0)
                throw new DomainException("Minimum stay in weeks cannot be negative.");

            _minimumStayInWeeks = weeks;
            return this;
        }

        public Builder WithAgeRange(int? ageFrom, int? ageTo)
        {
            _ageRange = AgeRange.Create(ageFrom, ageTo);
            return this;
        }

        public Builder WithCommitment(bool committed)
        {
            _isCommitted = committed;
            return this;
        }
        
        public Builder WithNonCommitment(bool nonCommitted)
        {
            _isNonCommitted = nonCommitted;
            return this;
        }

        public Accommodation Build()
        {
            Accommodation accommodation = new(_name, _accommodationTypeId)
            {
                IsActive = _isActive,
                MinimumStayInWeeks = _minimumStayInWeeks,
                AgeRange = _ageRange,
                IsCommitted = _isCommitted,
                IsNonCommitted = _isNonCommitted
            };

            return accommodation;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
