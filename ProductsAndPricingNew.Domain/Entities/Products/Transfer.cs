using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.Products;

public sealed class Transfer : AggregateRoot<int>, IProductDefinition
{
    public int DivisionId { get; private set; }
    public int UnitTypeId { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public int TransferTypeId { get; private set; }
    public int TransferPortId { get; private set; }
    public TimeWindow TimeWindow { get; private set; } = TimeWindow.Undefined;
    public ProductCategories Categories { get; private set; } = ProductCategories.Unassigned;
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;
    public OfferingsClosurePolicy ClosurePolicy { get; private set; } = OfferingsClosurePolicy.Open;

    private Transfer() { }

    private Transfer(int divisionId, int unitTypeId, int transferTypeId, int transferPortId, string name)
    {
        DivisionId = divisionId;
        UnitTypeId = unitTypeId;
        TransferTypeId = transferTypeId;
        TransferPortId = transferPortId;
        Name = name;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void SetIsActive(bool isActive) =>
        IsActive = isActive;

    public void WithTransferType(int transferTypeId) =>
        TransferTypeId = Guard.PositiveId(transferTypeId, nameof(TransferTypeId));

    public void WithTransferPort(int transferPortId) =>
        TransferPortId = Guard.PositiveId(transferPortId, nameof(TransferPortId));

    public void WithUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Transfer, unitType);
        UnitTypeId = unitType.Id;
    }

    public void WithTimeWindow(TimeOnly? from, TimeOnly? to) =>
        TimeWindow = TimeWindow.Create(from, to);

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
        private readonly int _transferTypeId;
        private readonly int _transferPortId;
        private readonly int _unitTypeId;

        private bool _isActive = true;
        private TimeWindow _timeWindow = TimeWindow.Undefined;
        private ProductCategories _categories = ProductCategories.Unassigned;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;
        private OfferingsClosurePolicy _closurePolicy = OfferingsClosurePolicy.Open;

        public Builder(int divisionId, string name, int transferTypeId, int transferPortId, UnitType unitType)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForProduct(ProductKind.Transfer, unitType);

            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _transferTypeId = Guard.PositiveId(transferTypeId, nameof(TransferTypeId));
            _transferPortId = Guard.PositiveId(transferPortId, nameof(TransferPortId));
            _unitTypeId = unitType.Id;
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithTimeWindow(TimeOnly? from, TimeOnly? to)
        {
            _timeWindow = TimeWindow.Create(from, to);
            return this;
        }

        public Builder WithCategories(int accountCategoryId, int productCategoryId)
        {
            _categories = ProductCategories.Create(accountCategoryId, productCategoryId);
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

        public Transfer Build()
        {
            Transfer transfer = new(_divisionId, _unitTypeId, _transferTypeId, _transferPortId, _name)
            {
                IsActive = _isActive,
                TimeWindow = _timeWindow,
                Categories = _categories,
                FinanceCodes = _financeCodes,
                ClosurePolicy = _closurePolicy
            };

            return transfer;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 100;
    }
}
