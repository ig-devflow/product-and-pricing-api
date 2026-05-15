using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Application.Common.Mapping;

internal static class AddressDtoMappingExtensions
{
    public static AddressDefinition ToDefinition(this AddressDto? dto) =>
        dto is null
            ? AddressDefinition.Empty
            : new AddressDefinition(dto.CountryId, dto.Street, dto.District, dto.City, dto.PostalCode);
}