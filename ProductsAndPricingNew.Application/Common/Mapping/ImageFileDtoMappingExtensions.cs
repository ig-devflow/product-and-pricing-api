using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Application.Common.Mapping;

internal static class ImageFileDtoMappingExtensions
{
    public static ImageFileDefinition ToDefinition(this ImageFileDto? dto) =>
        dto is null
            ? ImageFileDefinition.Empty
            : new ImageFileDefinition(dto.Data, dto.ContentType, dto.FileName);
}