using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

public readonly record struct FinanceCodes(string? GeneralLedgerCode, string? CostCentreCode) : IEmptyValueObject
{
    public static readonly FinanceCodes Unassigned = new(null, null);

    public bool IsEmpty => string.IsNullOrEmpty(GeneralLedgerCode) && string.IsNullOrEmpty(CostCentreCode);

    public static FinanceCodes Create(string? generalLedgerCode, string? costCentreCode)
    {
        string? normalizedLedgerCode = generalLedgerCode.AsOptionalDomainText(nameof(GeneralLedgerCode), Rules.MaxLength);
        string? normalizedCentreCode = costCentreCode.AsOptionalDomainText(nameof(CostCentreCode), Rules.MaxLength);

        return new FinanceCodes(normalizedLedgerCode, normalizedCentreCode);
    }

    public static FinanceCodes Create(FinanceCodesDefinition? definition) =>
        definition is null
            ? Unassigned
            : Create(definition.GeneralLedgerCode, definition.CostCentreCode);

    public static class Rules
    {
        public const int MaxLength = 50;
    }
}