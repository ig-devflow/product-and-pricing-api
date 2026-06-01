using AutoMapper;
using ProductsAndPricingNew.Application.Features.ProductCategory.Commands.CreateProductCategory;
using ProductsAndPricingNew.Application.Features.ProductCategory.Commands.UpdateProductCategory;

namespace ProductsAndPricingNew.AdminApi.Contracts.ProductCategory;

public sealed class ProductCategoryMappingProfile : Profile
{
    public ProductCategoryMappingProfile()
    {
        CreateMap<CreateProductCategoryRequest, CreateProductCategoryCommand>()
            .ConstructUsing(src => new CreateProductCategoryCommand(0, src.Name, src.IsActive));

        CreateMap<UpdateProductCategoryRequest, UpdateProductCategoryCommand>()
            .ConstructUsing(src => new UpdateProductCategoryCommand(0, src.Name, src.Version));
    }
}
