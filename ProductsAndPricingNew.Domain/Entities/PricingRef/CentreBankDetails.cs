using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class CentreBankDetails : IEquatable<CentreBankDetails>
{
    public string BeneficiaryName { get; } = null!;
    public string AccountNumber { get; } = null!;
    public string BankName { get; } = null!;
    public BankIdentifiers Identifiers { get; private init; } = BankIdentifiers.Empty;
    public Address BankAddress { get; private init; } = Address.Empty;
    public Address BeneficiaryBankAddress { get; private init; } = Address.Empty;
    public IntermediaryBank Intermediary { get; private init; } = IntermediaryBank.Empty;

    private CentreBankDetails() { }

    private CentreBankDetails(string beneficiaryName, string accountNumber, string bankName)
    {
        BeneficiaryName = beneficiaryName;
        AccountNumber = accountNumber;
        BankName = bankName;
    }

    public static CentreBankDetails Create(CentreBankDetailsDefinition? definition)
    {
        if (definition is null)
            throw new DomainException("CentreBankDetailsDefinition cannot be null.");

        var bankDetails = new CentreBankDetails(
            definition.BeneficiaryName.AsRequiredDomainText(nameof(BeneficiaryName), Rules.MaxLength),
            definition.AccountNumber.AsRequiredDomainText(nameof(AccountNumber), Rules.MaxLength),
            definition.BankName.AsRequiredDomainText(nameof(BankName), Rules.MaxLength))
        {
            Identifiers = BankIdentifiers.Create(definition.Identifiers),
            BankAddress = Address.Create(definition.BankAddress),
            BeneficiaryBankAddress = Address.Create(definition.BeneficiaryBankAddress),
            Intermediary = IntermediaryBank.Create(definition.Intermediary)
        };

        return bankDetails;
    }

    public bool Equals(CentreBankDetails? other) =>
        other is not null
        && BeneficiaryName == other.BeneficiaryName
        && AccountNumber == other.AccountNumber
        && BankName == other.BankName
        && Identifiers.Equals(other.Identifiers)
        && BankAddress.Equals(other.BankAddress)
        && BeneficiaryBankAddress.Equals(other.BeneficiaryBankAddress)
        && Intermediary.Equals(other.Intermediary);

    public override bool Equals(object? obj) => Equals(obj as CentreBankDetails);

    public override int GetHashCode() =>
        HashCode.Combine(BeneficiaryName, AccountNumber, BankName, Identifiers, BankAddress, BeneficiaryBankAddress, Intermediary);

    public static class Rules
    {
        public const int MaxLength = 100;
    }
}