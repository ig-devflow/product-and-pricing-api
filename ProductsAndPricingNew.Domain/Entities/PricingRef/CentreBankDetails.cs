using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed record CentreBankDetails : IEmptyValueObject
{
    public string? BeneficiaryName { get; private set; }
    public string? AccountNumber { get; private set; }
    public string? BankName { get; private set; }
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

    public bool IsEmpty =>
        BeneficiaryName is null &&
        AccountNumber is null &&
        BankName is null &&
        Iban is null &&
        SwiftCode is null &&
        BranchCode is null &&
        AbaRoutingNo is null &&
        AchAba is null &&
        BankAddress.IsEmpty &&
        BeneficiaryBankAddress.IsEmpty &&
        IntermediaryBankAddress.IsEmpty &&
        IntermediaryBankName is null &&
        IntermediarySwiftCode is null;

    public static CentreBankDetails Empty { get; } = new();
}