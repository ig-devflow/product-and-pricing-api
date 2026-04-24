using ProductsAndPricingNew.Domain.Base;
using ProductsAndPricingNew.Domain.Common.Errors;
using ProductsAndPricingNew.Domain.Common.Extensions;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Course : AggregateRoot<int>, IProductDefinition
{
    public int DivisionId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int CourseLanguageId { get; private set; }
    public int CourseIntensityId { get; private set; }
    public int UnitTypeId { get; private set; }
    public int? MinimumAge { get; private set; }
    public int? MinimumWeeks { get; private set; }
    public int? AccountCategoryId { get; private set; }
    public int? ProductCategoryId { get; private set; }
    public DateOnly? OfferingsClosureDate { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; }

    private Course() { }

    public Course(int id, int divisionId, string name, int courseLanguageId, int courseIntensityId, int unitTypeId)
    {
        Id = id;
        DivisionId = divisionId;
        CourseLanguageId = courseLanguageId;
        CourseIntensityId = courseIntensityId;
        UnitTypeId = unitTypeId;
        IsActive = true;
        FinanceCodes = new FinanceCodes(null, null);

        Rename(name);
    }

    public void Rename(string name) => Name = name.AsRequiredDomainText();
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
    public void ChangeLanguage(int courseLanguageId) => CourseLanguageId = courseLanguageId;
    public void ChangeIntensity(int courseIntensityId) => CourseIntensityId = courseIntensityId;
    public void ChangeUnitType(int unitTypeId) => UnitTypeId = unitTypeId;
    public void ChangeMinimumAge(int? value) => MinimumAge = value;
    public void ChangeMinimumWeeks(int? value) => MinimumWeeks = value;

    public void ChangeCategories(int? accountCategoryId, int? productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public void ChangeFinanceCodes(FinanceCodes codes) => FinanceCodes = codes;
    public void ChangeOfferingsClosureDate(DateOnly? value) => OfferingsClosureDate = value;
}