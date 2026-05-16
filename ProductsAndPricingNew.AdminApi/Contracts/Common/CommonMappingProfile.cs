using AutoMapper;
using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.AdminApi.Contracts.Common;

public sealed class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        CreateMap<AddressRequest, AddressDto>();

        CreateMap<ImageFileRequest, ImageFileDto>();

        CreateMap<TextContentRequest, TextContentDto>();
    }
}