using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
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

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void WithCategories(int accountCategoryId, int productCategoryId) =>
        Categories = ProductCategories.Create(accountCategoryId, productCategoryId);

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

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithCategories(int accountCategoryId, int productCategoryId)
        {
            _categories = ProductCategories.Create(accountCategoryId, productCategoryId);
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
