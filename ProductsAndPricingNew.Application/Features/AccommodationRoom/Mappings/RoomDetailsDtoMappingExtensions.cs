using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;
using ProductsAndPricingNew.Domain.Entities.Products.Definitions;
using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Application.Features.AccommodationRoom.Mappings;

internal static class RoomDetailsDtoMappingExtensions
{
    public static RoomDetailsDefinition ToDefinition(this RoomDetailsDto dto)
        => new(dto.RoomTypeId, dto.BoardTypeId, dto.BathroomTypeId, dto.RoomGradeId);
}