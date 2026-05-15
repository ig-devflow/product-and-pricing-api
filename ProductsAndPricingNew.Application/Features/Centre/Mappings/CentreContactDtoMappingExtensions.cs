using ProductsAndPricingNew.Application.Common.Mapping;
using ProductsAndPricingNew.Application.Features.Centre.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;

namespace ProductsAndPricingNew.Application.Features.Centre.Mappings;

internal static class CentreContactDtoMappingExtensions
{
    public static IEnumerable<CentreContactDefinition> ToDefinitions(this IEnumerable<CentreContactDto> dtos) =>
        dtos.Select(ToDefinition);

    private static CentreContactDefinition ToDefinition(this CentreContactDto dto) =>
        new(dto.ContactType, dto.Name, dto.Email, dto.SignatureImage.ToDefinition());
}