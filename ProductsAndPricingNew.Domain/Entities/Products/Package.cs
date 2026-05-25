using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Package : AggregateRoot<int>, IProductDefinition
{
    private readonly List<PackageItem> _items = new();

    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }
    public Percentage Commission { get; private set; } = Percentage.Zero;
    public AgeRange AgeRange { get; private set; } = AgeRange.Open;
    public int? MinimumWeeks { get; private set; }
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;
    public OfferingsClosurePolicy ClosurePolicy { get; private set; } = OfferingsClosurePolicy.Open;

    public IReadOnlyCollection<PackageItem> Items => _items.AsReadOnly();

    private Package() { }

    private Package(int divisionId, int unitTypeId, string name)
    {
        DivisionId = divisionId;
        UnitTypeId = unitTypeId;
        Name = name;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeDescription(string? description) =>
        Description = description.AsOptionalText();

    public void ChangeIsActive(bool isActive) =>
        IsActive = isActive;

    public void ChangeUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Package, unitType);
        UnitTypeId = unitType.Id;
    }

    public void ChangeCommission(decimal commissionPercentage) =>
        Commission = Percentage.Create(commissionPercentage);

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

    public void AddItem(ProductRef product, decimal percentage)
    {
        EnsureNotSelfReference(product);

        if (_items.Any(x => x.Product == product))
            throw new DomainException("Duplicate package item.");

        _items.Add(new PackageItem(product, Percentage.Create(percentage)));
        EnsureBreakdownDoesNotExceed100();
    }

    public void ChangeItemPercentage(ProductRef product, decimal percentage)
    {
        PackageItem item = _items.SingleOrDefault(x => x.Product == product)
                           ?? throw new DomainException("Package item not found.");

        item.ChangePercentage(Percentage.Create(percentage));
        EnsureBreakdownDoesNotExceed100();
    }

    public void RemoveItem(ProductRef product)
    {
        PackageItem item = _items.SingleOrDefault(x => x.Product == product)
                           ?? throw new DomainException("Package item not found.");

        _items.Remove(item);
    }

    public void EnsureBreakdownTotalEquals100()
    {
        decimal total = _items.Sum(x => x.PriceBreakdown.Value);

        if (Math.Abs(total - 100m) > 0.01m)
            throw new DomainException($"Total percentage breakdown must equal 100%, current total is {total}%.");
    }

    private void EnsureBreakdownDoesNotExceed100()
    {
        if (_items.Sum(x => x.PriceBreakdown.Value) > 100m)
            throw new DomainException("Package breakdown total cannot exceed 100%.");
    }

    private void EnsureNotSelfReference(ProductRef product)
    {
        if (product.Kind == ProductKind.Package && Id != 0 && product.Id == Id)
            throw new DomainException("Package cannot include itself.");
    }

    public sealed class Builder
    {
        private readonly int _divisionId;
        private readonly string _name;
        private readonly int _unitTypeId;

        private bool _isActive = true;
        private string? _description;
        private Percentage _commission = Percentage.Zero;
        private AgeRange _ageRange = AgeRange.Open;
        private int? _minimumWeeks;
        private ProductCategories _categories = ProductCategories.Unassigned;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;
        private readonly List<(ProductRef Product, decimal Percentage)> _items = new();

        public Builder(int divisionId, string name, UnitType unitType)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Package, unitType);

            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _unitTypeId = unitType.Id;
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithDescription(string? value)
        {
            _description = value.AsOptionalText();
            return this;
        }

        public Builder WithCommission(decimal commissionPercentage)
        {
            _commission = Percentage.Create(commissionPercentage);
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

        public Builder WithItem(ProductRef product, decimal percentage)
        {
            _items.Add((product, percentage));
            return this;
        }

        public Package Build()
        {
            Package package = new(_divisionId, _unitTypeId, _name)
            {
                IsActive = _isActive,
                Description = _description,
                Commission = _commission,
                AgeRange = _ageRange,
                MinimumWeeks = _minimumWeeks,
                Categories = _categories,
                FinanceCodes = _financeCodes
            };

            foreach ((ProductRef product, decimal percentage) in _items)
                package.AddItem(product, percentage);

            return package;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
