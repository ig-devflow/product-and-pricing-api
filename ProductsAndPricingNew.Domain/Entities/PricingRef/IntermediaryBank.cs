using ProductsAndPricingNew.Domain.Common.Text;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef;

public sealed class IntermediaryBank : IEmptyValueObject, IEquatable<IntermediaryBank>
{
    public string? BankName { get; }
    public string? SwiftCode { get; }
    public Address BankAddress { get; } = Address.Empty;

    private IntermediaryBank() { }

    private IntermediaryBank(string? bankName, string? swiftCode, Address bankAddress)
    {
        BankName = bankName;
        SwiftCode = swiftCode;
        BankAddress = bankAddress;
    }

    public static IntermediaryBank Create(IntermediaryBankDefinition? definition)
    {
        if (definition is null || definition.IsEmpty)
            return Empty;

        return new IntermediaryBank(
            definition.BankName.AsOptionalDomainText(nameof(BankName), Rules.MaxLength),
            definition.SwiftCode.AsOptionalDomainText(nameof(SwiftCode), Rules.MaxLength),
            Address.Create(definition.BankAddress));
    }

    public bool IsEmpty =>
        BankName is null &&
        SwiftCode is null &&
        BankAddress.IsEmpty;

    public static IntermediaryBank Empty { get; } = new();

    public bool Equals(IntermediaryBank? other) =>
        other is not null
        && BankName == other.BankName
        && SwiftCode == other.SwiftCode
        && BankAddress.Equals(other.BankAddress);

    public override bool Equals(object? obj) => Equals(obj as IntermediaryBank);

    public override int GetHashCode() => HashCode.Combine(BankName, SwiftCode, BankAddress);

    public static class Rules
    {
        public const int MaxLength = 100;
    }
}