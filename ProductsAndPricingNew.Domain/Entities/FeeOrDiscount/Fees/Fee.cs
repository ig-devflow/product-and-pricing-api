using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;
using ProductsAndPricingNew.Domain.UnitOfMeasure;

namespace ProductsAndPricingNew.Domain.Entities.FeeOrDiscount.Fees;

public sealed class Fee : AggregateRoot<int>
{
    public string Name { get; private set; } = null!;
    public int UnitTypeId { get; private set; }
    public int DivisionId { get; private set; }
    public bool IsActive { get; private set; }
    public FinanceCodes FinanceCodes { get; private set; } = FinanceCodes.Unassigned;

    private Fee() { }

    private Fee(string name, int unitTypeId, int divisionId)
    {
        Name = name;
        UnitTypeId = unitTypeId;
        DivisionId = divisionId;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    public void WithUnitType(UnitType unitType)
    {
        UnitTypePolicy.EnsureAllowedForFee(unitType);
        UnitTypeId = unitType.Id;
    }

    public void SetIsActive(bool isActive) => IsActive = isActive;

    public void WithFinanceCodes(string? generalLedgerCode, string? costCentreCode) =>
        FinanceCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);

    public sealed class Builder
    {
        private readonly string _name;
        private readonly int _unitTypeId;
        private readonly int _divisionId;

        private bool _isActive = true;
        private FinanceCodes _financeCodes = FinanceCodes.Unassigned;

        public Builder(string name, UnitType unitType, int divisionId)
        {
            ArgumentNullException.ThrowIfNull(unitType);
            UnitTypePolicy.EnsureAllowedForFee(unitType);

            _name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);
            _unitTypeId = unitType.Id;
            _divisionId = Guard.PositiveId(divisionId, nameof(DivisionId));
        }

        public Builder SetIsActive(bool value)
        {
            _isActive = value;
            return this;
        }

        public Builder WithFinanceCodes(string? generalLedgerCode, string? costCentreCode)
        {
            _financeCodes = FinanceCodes.Create(generalLedgerCode, costCentreCode);
            return this;
        }

        public Fee Build()
        {
            Fee fee = new(_name, _unitTypeId, _divisionId)
            {
                IsActive = _isActive,
                FinanceCodes = _financeCodes
            };

            return fee;
        }
    }

    public static class Rules
    {
        public const int NameMaxLength = 200;
    }
}
