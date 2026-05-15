using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class BankIdentifiers : IEmptyValueObject, IEquatable<BankIdentifiers>
{
    public string? Iban { get; }
    public string? SwiftCode { get; }
    public string? BranchCode { get; }
    public string? AbaRoutingNo { get; }
    public string? AchAba { get; }

    private BankIdentifiers() { }

    private BankIdentifiers(
        string? iban,
        string? swiftCode,
        string? branchCode,
        string? abaRoutingNo,
        string? achAba)
    {
        Iban = iban;
        SwiftCode = swiftCode;
        BranchCode = branchCode;
        AbaRoutingNo = abaRoutingNo;
        AchAba = achAba;
    }

    public static BankIdentifiers Create(BankIdentifiersDefinition? definition)
    {
        if (definition is null || definition.IsEmpty)
            return Empty;

        return new BankIdentifiers(
            definition.Iban.AsOptionalDomainText(nameof(Iban), Rules.MaxLength),
            definition.SwiftCode.AsOptionalDomainText(nameof(SwiftCode), Rules.MaxLength),
            definition.BranchCode.AsOptionalDomainText(nameof(BranchCode), Rules.MaxLength),
            definition.AbaRoutingNo.AsOptionalDomainText(nameof(AbaRoutingNo), Rules.MaxLength),
            definition.AchAba.AsOptionalDomainText(nameof(AchAba), Rules.MaxLength));
    }

    public bool IsEmpty =>
        Iban is null &&
        SwiftCode is null &&
        BranchCode is null &&
        AbaRoutingNo is null &&
        AchAba is null;

    public static BankIdentifiers Empty { get; } = new();

    public bool Equals(BankIdentifiers? other) =>
        other is not null
        && Iban == other.Iban
        && SwiftCode == other.SwiftCode
        && BranchCode == other.BranchCode
        && AbaRoutingNo == other.AbaRoutingNo
        && AchAba == other.AchAba;

    public override bool Equals(object? obj) => Equals(obj as BankIdentifiers);

    public override int GetHashCode() =>
        HashCode.Combine(Iban, SwiftCode, BranchCode, AbaRoutingNo, AchAba);

    public static class Rules
    {
        public const int MaxLength = 100;
    }
}