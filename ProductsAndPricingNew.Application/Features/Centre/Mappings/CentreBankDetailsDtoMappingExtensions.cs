using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Application.Features.Centre.Mappings;

internal static class CentreBankDetailsDtoMappingExtensions
{
    public static CentreBankDetailsDefinition ToDefinition(this CentreBankDetailsDto? dto)
    {
        BankIdentifiersDefinition identifiers = new(dto?.Iban, dto?.SwiftCode, dto?.BranchCode, dto?.AbaRoutingNo, dto?.AchAba);

        IntermediaryBankDefinition intermediary = new(
            dto?.IntermediaryBankName,
            dto?.IntermediarySwiftCode,
            dto?.IntermediaryBankAddress.ToDefinition() ?? AddressDefinition.Empty);

        return new CentreBankDetailsDefinition(
            dto?.BeneficiaryName,
            dto?.AccountNumber,
            dto?.BankName,
            identifiers,
            dto?.BankAddress.ToDefinition() ?? AddressDefinition.Empty,
            dto?.BeneficiaryBankAddress.ToDefinition() ?? AddressDefinition.Empty,
            intermediary);
    }
}