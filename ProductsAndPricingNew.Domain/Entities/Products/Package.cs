using ProductsAndPricingNew.Domain.Base;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Package : AggregateRoot<int>, IProductDefinition
{
    private readonly List<PackageItem> _items = new();

    private Package() { }

    public Package(int id, int divisionId, string name, int unitTypeId)
    {
        Id = id;
        DivisionId = divisionId;
        UnitTypeId = unitTypeId;
        IsActive = true;
        FinanceCodes = new FinanceCodes(null, null);

        Rename(name);
    }

    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }
    public decimal CommissionPercentage { get; private set; }
    public int? MinimumAge { get; private set; }
    public int? MaximumAge { get; private set; }
    public int? MinimumWeeks { get; private set; }
    public int? AccountCategoryId { get; private set; }
    public int? ProductCategoryId { get; private set; }
    public DateOnly? OfferingsClosureDate { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; }

    public IReadOnlyCollection<PackageItem> Items => _items.AsReadOnly();

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Package name is required");

        Name = name.Trim();
    }

    public void ChangeDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;

    public void ChangeUnitType(int unitTypeId)
    {
        if (unitTypeId <= 0)
            throw new DomainException("UnitTypeId must be greater than zero");

        UnitTypeId = unitTypeId;
    }

    public void SetCommissionPercentage(decimal commissionPercentage)
    {
        if (commissionPercentage < 0 || commissionPercentage > 100)
            throw new DomainException("Commission percentage must be between 0 and 100");

        CommissionPercentage = commissionPercentage;
    }

    public void SetAgeRange(int? minimumAge, int? maximumAge)
    {
        if (minimumAge.HasValue && minimumAge.Value < 0)
            throw new DomainException("Minimum age cannot be negative");

        if (maximumAge.HasValue && maximumAge.Value < 0)
            throw new DomainException("Maximum age cannot be negative");

        if (minimumAge.HasValue && maximumAge.HasValue && minimumAge > maximumAge)
            throw new DomainException("Minimum age cannot be greater than maximum age");

        MinimumAge = minimumAge;
        MaximumAge = maximumAge;
    }

    public void SetMinimumWeeks(int? weeks)
    {
        if (weeks.HasValue && weeks.Value < 0)
            throw new DomainException("Minimum weeks must be 0 or greater");

        MinimumWeeks = weeks;
    }

    public void ChangeCategories(int? accountCategoryId, int? productCategoryId)
    {
        AccountCategoryId = accountCategoryId;
        ProductCategoryId = productCategoryId;
    }

    public void ChangeFinanceCodes(FinanceCodes codes) => FinanceCodes = codes;
    public void ChangeOfferingsClosureDate(DateOnly? value) => OfferingsClosureDate = value;

    public void AddItem(ProductRef product, decimal percentage)
    {
        if (product.Kind == ProductKind.Package && product.Id == Id)
            throw new DomainException("Package cannot include itself");

        if (_items.Any(x => x.ProductKind == product.Kind && x.ProductDefinitionId == product.Id))
            throw new DomainException("Duplicate package item");

        _items.Add(new PackageItem(product, percentage));
        ValidatePercentagesDoNotExceed100();
    }

    public void ChangeItemPercentage(ProductRef product, decimal percentage)
    {
        PackageItem item = _items.SingleOrDefault(x => x.ProductKind == product.Kind && x.ProductDefinitionId == product.Id)
                           ?? throw new DomainException("Package item not found");

        item.ChangePercentage(percentage);
        ValidatePercentagesDoNotExceed100();
    }

    public void RemoveItem(ProductRef product)
    {
        PackageItem item = _items.SingleOrDefault(x => x.Product == product)
                           ?? throw new DomainException("Package item not found");

        _items.Remove(item);
    }

    public void EnsureBreakdownTotalEquals100()
    {
        decimal total = _items.Sum(x => x.Percentage);

        if (Math.Abs(total - 100m) > 0.01m)
            throw new DomainException($"Total percentage breakdown must equal 100%, current total is {total}%");
    }

    private void ValidatePercentagesDoNotExceed100()
    {
        decimal total = _items.Sum(x => x.Percentage);

        if (total > 100m)
            throw new DomainException("Package breakdown total cannot exceed 100%");
    }
}

public sealed class PackageItem : Entity<int>
{
    private PackageItem() { }

    internal PackageItem(ProductRef product, decimal percentage)
    {
        ProductKind = product.Kind;
        ProductDefinitionId = product.Id;
        ChangePercentage(percentage);
    }

    public ProductKind ProductKind { get; private set; }
    public int ProductDefinitionId { get; private set; }
    public decimal Percentage { get; private set; }

    public ProductRef Product => new(ProductKind, ProductDefinitionId);

    internal void ChangePercentage(decimal percentage)
    {
        if (percentage <= 0 || percentage > 100)
            throw new DomainException("Percentage breakdown must be between 0 and 100");

        Percentage = percentage;
    }
}