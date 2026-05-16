using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Application.Common.Mapping;

internal static class TextContentDtoMappingExtensions
{
    public static IEnumerable<TextContentDefinition> ToDefinitions(this IEnumerable<TextContentDto> dtos) =>
        dtos.Select(ToDefinition);

    private static TextContentDefinition ToDefinition(this TextContentDto dto) =>
        new(dto.ContentTemplateId, dto.AudienceId, dto.Content, dto.Format);
}