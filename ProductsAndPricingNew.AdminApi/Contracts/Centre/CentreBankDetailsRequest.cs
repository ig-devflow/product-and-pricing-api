using ProductsAndPricingNew.AdminApi.Contracts.Common;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreBankDetailsRequest(
    string BeneficiaryName,
    string AccountNumber,
    string BankName,
    string? Iban,
    string? SwiftCode,
    string? BranchCode,
    string? AbaRoutingNo,
    string? AchAba,
    AddressRequest BankAddress,
    AddressRequest BeneficiaryBankAddress,
    AddressRequest IntermediaryBankAddress,
    string? IntermediaryBankName,
    string? IntermediarySwiftCode
);