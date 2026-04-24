namespace ProductsAndPricingNew.Domain.Entities.Products;

public readonly struct FinanceCodes : IEquatable<FinanceCodes>
{
    public string? GeneralLedgerCode { get; }
    public string? CostCentreCode { get; }

    public FinanceCodes(string? generalLedgerCode, string? costCentreCode)
    {
        GeneralLedgerCode = generalLedgerCode;
        CostCentreCode = costCentreCode;
    }

    public bool Equals(FinanceCodes other)
    {
        return string.Equals(GeneralLedgerCode, other.GeneralLedgerCode, StringComparison.Ordinal)
               && string.Equals(CostCentreCode, other.CostCentreCode, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is FinanceCodes other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GeneralLedgerCode, CostCentreCode);
    }

    public override string ToString()
    {
        return $"GeneralLedgerCode: {GeneralLedgerCode ?? "-"}, CostCentre: {CostCentreCode ?? "-"}";
    }

    public static bool operator ==(FinanceCodes left, FinanceCodes right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(FinanceCodes left, FinanceCodes right)
    {
        return !left.Equals(right);
    }
}