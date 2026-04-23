using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class AddOn : AggregateRoot<int>, IProductDefinition
{
    private AddOn() { }

    public AddOn(int id, int divisionId, string name, int addOnTypeId, int unitTypeId)
    {
        Id = id;
        DivisionId = divisionId;
        AddOnTypeId = addOnTypeId;
        UnitTypeId = unitTypeId;
        IsActive = true;
        FinanceCodes = new FinanceCodes(null, null);

        Rename(name);
    }

    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int AddOnTypeId { get; private set; }
    public int? MinimumAge { get; private set; }
    public int? AccountCategoryId { get; private set; }
    public int? ProductCategoryId { get; private set; }
    public int? OneToOneLessonsPerWeek { get; private set; }

    public DateOnly? OfferingsClosureDate { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("AddOn name is required");

        Name = name.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ChangeType(int addOnTypeId)
    {
        if (addOnTypeId <= 0)
            throw new DomainException("AddOnTypeId must be greater than zero");

        AddOnTypeId = addOnTypeId;
    }

    public void ChangeUnitType(int unitTypeId)
    {
        if (unitTypeId <= 0)
            throw new DomainException("UnitTypeId must be greater than zero");

        UnitTypeId = unitTypeId;
    }

    public void ChangeMinimumAge(int? value)
    {
        if (value.HasValue && value.Value < 0)
            throw new DomainException("Minimum age cannot be negative");

        MinimumAge = value;
    }

    public void SetOneToOneLessonsPerWeek(int? lessonsPerWeek)
    {
        if (lessonsPerWeek.HasValue && lessonsPerWeek.Value < 1)
            throw new DomainException("Number of lessons per week must be at least 1");

        OneToOneLessonsPerWeek = lessonsPerWeek;
    }

    public void ClearOneToOneLessonsPerWeek() => OneToOneLessonsPerWeek = null;

    public void ChangeCategories(int? accountCategoryId, int? productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public void ChangeFinanceCodes(FinanceCodes codes) => FinanceCodes = codes;
    public void ChangeOfferingsClosureDate(DateOnly? value) => OfferingsClosureDate = value;
}