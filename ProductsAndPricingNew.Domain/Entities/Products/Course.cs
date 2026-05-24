using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Course : AggregateRoot<int>, IProductDefinition
{
    public int DivisionId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int CourseLanguageId { get; private set; }
    public int CourseIntensityId { get; private set; }
    public int UnitTypeId { get; private set; }
    public AgeRange AgeRange { get; private set; } = AgeRange.Open;
    public int? MinimumWeeks { get; private set; }
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;
    public OfferingsClosurePolicy ClosurePolicy { get; private set; } = OfferingsClosurePolicy.Open;

    private Course() { }

    private Course(int divisionId, string name, int courseLanguageId, int courseIntensityId, int unitTypeId)
    {
        DivisionId = divisionId;
        Name = name;
        CourseLanguageId = courseLanguageId;
        CourseIntensityId = courseIntensityId;
        UnitTypeId = unitTypeId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeIsActive(bool isActive) =>
        IsActive = isActive;

    public void ChangeLanguage(int courseLanguageId) =>
        CourseLanguageId = Guard.PositiveId(courseLanguageId, nameof(CourseLanguageId));

    public void ChangeIntensity(int courseIntensityId) =>
        CourseIntensityId = Guard.PositiveId(courseIntensityId, nameof(CourseIntensityId));

    public void ChangeUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Course, unitType);
        UnitTypeId = unitType.Id;
    }

    public void ChangeAgeRange(AgeRangeDefinition? definition) =>
        AgeRange = AgeRange.Create(definition);

    public void ChangeMinimumWeeks(int? weeks)
    {
        if (weeks is < 0)
            throw new DomainException("Minimum weeks must be 0 or greater.");

        MinimumWeeks = weeks;
    }

    public void ChangeCategories(ProductCategoriesDefinition? definition) =>
        Categories = ProductCategories.Create(definition);

    public void ChangeFinanceCodes(FinanceCodesDefinition? definition) =>
        FinanceCodes = FinanceCodes.Create(definition);

    public void ChangeClosurePolicy(DateOnly date) =>
        ClosurePolicy = OfferingsClosurePolicy.Create(date);

    public sealed class Builder
    {
        private readonly int _divisionId;
        private readonly string _name;
        private readonly int _courseLanguageId;
        private readonly int _courseIntensityId;
        private readonly int _unitTypeId;

        private bool _isActive = true;
        private AgeRange _ageRange = AgeRange.Open;
        private int? _minimumWeeks;
        private ProductCategories _categories = ProductCategories.Unassigned;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;

        public Builder(int divisionId, string name, int courseLanguageId, int courseIntensityId, UnitType unitType)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Course, unitType);

            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _courseLanguageId = Guard.PositiveId(courseLanguageId, nameof(CourseLanguageId));
            _courseIntensityId = Guard.PositiveId(courseIntensityId, nameof(CourseIntensityId));
            _unitTypeId = unitType.Id;
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithAgeRange(AgeRangeDefinition? definition)
        {
            _ageRange = AgeRange.Create(definition);
            return this;
        }

        public Builder WithMinimumWeeks(int? weeks)
        {
            if (weeks is < 0)
                throw new DomainException("Minimum weeks must be 0 or greater.");

            _minimumWeeks = weeks;
            return this;
        }

        public Builder WithCategories(ProductCategoriesDefinition? definition)
        {
            _categories = ProductCategories.Create(definition);
            return this;
        }

        public Builder WithFinanceCodes(FinanceCodesDefinition? definition)
        {
            _financeCodes = FinanceCodes.Create(definition);
            return this;
        }

        public Course Build()
        {
            Course course = new(_divisionId, _name, _courseLanguageId, _courseIntensityId, _unitTypeId)
            {
                IsActive = _isActive,
                AgeRange = _ageRange,
                MinimumWeeks = _minimumWeeks,
                Categories = _categories,
                FinanceCodes = _financeCodes
            };

            return course;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
