using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.CancellationFees;

public sealed class CancellationFee : AggregateRoot<int>
{
    public int DivisionId { get; private set; }

    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;

    private CancellationFee() { }

    private CancellationFee(string name, int divisionId)
    {
        Name = name;
        DivisionId = divisionId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void ChangeIsActive(bool isActive) =>
        IsActive = isActive;

    public void ChangeCategories(ProductCategoriesDefinition? definition) =>
        Categories = ProductCategories.Create(definition);

    public sealed class Builder
    {
        private readonly string _name;
        private readonly int _divisionId;

        private bool _isActive = true;
        private ProductCategories _categories = ProductCategories.Unassigned;

        public Builder(string name, int divisionId)
        {
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
        }

        public Builder IsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithCategories(ProductCategoriesDefinition? definition)
        {
            _categories = ProductCategories.Create(definition);
            return this;
        }

        public CancellationFee Build()
        {
            CancellationFee fee = new(_name, _divisionId)
            {
                IsActive = _isActive,
                Categories = _categories
            };

            return fee;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 200;
    }
}
