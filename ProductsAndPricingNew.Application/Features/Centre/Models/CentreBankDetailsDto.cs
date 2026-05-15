using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreBankDetailsDto(
    string BeneficiaryName,
    string AccountNumber,
    string BankName,
    string? Iban,
    string? SwiftCode,
    string? BranchCode,
    string? AbaRoutingNo,
    string? AchAba,
    AddressDto BankAddress,
    AddressDto BeneficiaryBankAddress,
    AddressDto IntermediaryBankAddress,
    string? IntermediaryBankName,
    string? IntermediarySwiftCode
);