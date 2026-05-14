using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed record CentreBankDetails : IEmptyValueObject
{
    public string BeneficiaryName { get; private set; }
    public string AccountNumber { get; private set; }
    public string BankName { get; private set; }
    public string? Iban { get; private set; }
    public string? SwiftCode { get; private set; }
    public string? BranchCode { get; private set; }
    public string? AbaRoutingNo { get; private set; }
    public string? AchAba { get; private set; }
    public Address BankAddress { get; private set; } = Address.Empty;
    public Address BeneficiaryBankAddress { get; private set; } = Address.Empty;
    public Address IntermediaryBankAddress { get; private set; } = Address.Empty;
    public string? IntermediaryBankName { get; private set; }
    public string? IntermediarySwiftCode { get; private set; }

    private CentreBankDetails() { }

    private CentreBankDetails(string beneficiaryName, string accountNumber, string bankName)
    {
        BeneficiaryName = beneficiaryName;
        AccountNumber = accountNumber;
        BankName = bankName;
    }

    public static CentreBankDetails Create(
        string beneficiaryName,
        string accountNumber,
        string bankName,
        string? iban,
        string? swiftCode,
        string? branchCode,
        string? abaRoutingNo,
        string? achAba,
        Address bankAddress,
        Address beneficiaryBankAddress,
        Address intermediaryBankAddress,
        string? intermediaryBankName,
        string? intermediarySwiftCode)
    {

        var centreBankDetails = new CentreBankDetails(
            beneficiaryName.AsRequiredDomainText(nameof(beneficiaryName), Rules.MaxLength),
            accountNumber.AsRequiredDomainText(nameof(accountNumber), Rules.MaxLength),
            bankName.AsRequiredDomainText(nameof(bankName), Rules.MaxLength)
        )
        {
            Iban = iban.AsOptionalDomainText(nameof(iban), Rules.MaxLength),
            SwiftCode = swiftCode.AsOptionalDomainText(nameof(swiftCode), Rules.MaxLength),
            BranchCode = branchCode.AsOptionalDomainText(nameof(branchCode), Rules.MaxLength),
            AbaRoutingNo = abaRoutingNo.AsOptionalDomainText(nameof(abaRoutingNo), Rules.MaxLength),
            AchAba = achAba.AsOptionalDomainText(nameof(achAba), Rules.MaxLength),
            BankAddress = bankAddress,
            BeneficiaryBankAddress = beneficiaryBankAddress,
            IntermediaryBankAddress = intermediaryBankAddress,
            IntermediaryBankName = intermediaryBankName.AsOptionalDomainText(nameof(IntermediaryBankName), Rules.MaxLength),
            IntermediarySwiftCode = intermediarySwiftCode.AsOptionalDomainText(nameof(intermediarySwiftCode), Rules.MaxLength),
        };

        return centreBankDetails;
    }

    // public bool IsEmpty =>
    //     BeneficiaryName is null &&
    //     AccountNumber is null &&
    //     BankName is null &&
    //     Iban is null &&
    //     SwiftCode is null &&
    //     BranchCode is null &&
    //     AbaRoutingNo is null &&
    //     AchAba is null &&
    //     BankAddress.IsEmpty &&
    //     BeneficiaryBankAddress.IsEmpty &&
    //     IntermediaryBankAddress.IsEmpty &&
    //     IntermediaryBankName is null &&
    //     IntermediarySwiftCode is null;
    public bool IsEmpty =>
        BeneficiaryName is null &&
        AccountNumber is null &&
        BankName is null;

    public static CentreBankDetails Empty { get; } = new();

    public static class Rules
    {
        public const int MaxLength = 100;
    }
}