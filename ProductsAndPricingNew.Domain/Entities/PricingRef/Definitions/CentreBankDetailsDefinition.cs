using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;

public sealed record CentreBankDetailsDefinition(
    string? BeneficiaryName,
    string? AccountNumber,
    string? BankName,
    BankIdentifiersDefinition Identifiers,
    AddressDefinition BankAddress,
    AddressDefinition BeneficiaryBankAddress,
    IntermediaryBankDefinition Intermediary);

public sealed record BankIdentifiersDefinition(
    string? Iban,
    string? SwiftCode,
    string? BranchCode,
    string? AbaRoutingNo,
    string? AchAba)
{
    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(Iban)
        && string.IsNullOrWhiteSpace(SwiftCode)
        && string.IsNullOrWhiteSpace(BranchCode)
        && string.IsNullOrWhiteSpace(AbaRoutingNo)
        && string.IsNullOrWhiteSpace(AchAba);

    public static BankIdentifiersDefinition Empty { get; } = new(null, null, null, null, null);
}

public sealed record IntermediaryBankDefinition(
    string? BankName,
    string? SwiftCode,
    AddressDefinition BankAddress)
{
    public bool IsEmpty =>
        string.IsNullOrWhiteSpace(BankName)
        && string.IsNullOrWhiteSpace(SwiftCode)
        && BankAddress.IsEmpty;

    public static IntermediaryBankDefinition Empty { get; } =
        new(null, null, AddressDefinition.Empty);
}