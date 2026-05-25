using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class AddOn : AggregateRoot<int>, IProductDefinition
{
    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public AddOnType Type { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Open;
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;
    public int? OneToOneLessonsPerWeek { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;
    public OfferingsClosurePolicy ClosurePolicy { get; private set; } = OfferingsClosurePolicy.Open;

    private AddOn() { }

    private AddOn(int divisionId, string name, AddOnType type, int unitTypeId)
    {
        DivisionId = divisionId;
        Name = name;
        Type = type;
        UnitTypeId = unitTypeId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void WithType(AddOnType type)
    {
        Type = type;

        if (type != AddOnType.OneToOneCourse)
            OneToOneLessonsPerWeek = null;
    }

    public void WithUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForProduct(ProductKind.AddOn, unitType);
        UnitTypeId = unitType.Id;
    }

    public void WithAgeRange(int? ageFrom, int? ageTo) =>
        AgeRange = AgeRange.Create(ageFrom, ageTo);

    public void WithOneToOneLessonsPerWeek(int? lessonsPerWeek)
    {
        if (Type != AddOnType.OneToOneCourse)
        {
            OneToOneLessonsPerWeek = null;
            return;
        }

        if (lessonsPerWeek is null or < 1)
            throw new DomainException("OneToOneCourse AddOn requires a positive lessons-per-week value.");

        OneToOneLessonsPerWeek = lessonsPerWeek;
    }

    public void WithCategories(int accountCategoryId, int productCategoryId) =>
        Categories = ProductCategories.Create(accountCategoryId, productCategoryId);

    public void WithFinanceCodes(string? generalLedgerCode, string? costCentreCode) =>
        FinanceCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);

    public void WithClosurePolicy(DateOnly? date) =>
        ClosurePolicy = OfferingsClosurePolicy.Create(date);

    public sealed class Builder
    {
        private readonly int _divisionId;
        private readonly string _name;
        private readonly AddOnType _type;
        private readonly int _unitTypeId;

        private bool _isActive = true;
        private AgeRange _ageRange = AgeRange.Open;
        private ProductCategories _categories = ProductCategories.Unassigned;
        private int? _oneToOneLessonsPerWeek;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;
        private OfferingsClosurePolicy _closurePolicy = OfferingsClosurePolicy.Open;

        public Builder(int divisionId, string name, AddOnType type, UnitType unitType)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForProduct(ProductKind.AddOn, unitType);

            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _type = type;
            _unitTypeId = unitType.Id;
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithAgeRange(int? ageFrom, int? ageTo)
        {
            _ageRange = AgeRange.Create(ageFrom, ageTo);
            return this;
        }

        public Builder WithCategories(int accountCategoryId, int productCategoryId)
        {
            _categories = ProductCategories.Create(accountCategoryId, productCategoryId);
            return this;
        }

        public Builder WithOneToOneLessonsPerWeek(int? lessonsPerWeek)
        {
            if (_type != AddOnType.OneToOneCourse)
            {
                _oneToOneLessonsPerWeek = null;
                return this;
            }

            if (lessonsPerWeek < 1)
                throw new DomainException("OneToOneCourse AddOn requires a positive lessons-per-week value.");

            _oneToOneLessonsPerWeek = lessonsPerWeek;
            return this;
        }

        public Builder WithFinanceCodes(string? generalLedgerCode, string? costCentreCode)
        {
            _financeCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);
            return this;
        }

        public Builder WithClosurePolicy(DateOnly? value)
        {
            _closurePolicy = OfferingsClosurePolicy.Create(value);
            return this;
        }

        public AddOn Build()
        {
            if (_type == AddOnType.OneToOneCourse && _oneToOneLessonsPerWeek is null)
                throw new DomainException(
                    "OneToOneCourse AddOn requires OneToOneLessonsPerWeek to be set via the Builder.");

            AddOn addOn = new(_divisionId, _name, _type, _unitTypeId)
            {
                IsActive = _isActive,
                AgeRange = _ageRange,
                Categories = _categories,
                OneToOneLessonsPerWeek = _oneToOneLessonsPerWeek,
                FinanceCodes = _financeCodes,
                ClosurePolicy = _closurePolicy
            };

            return addOn;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
