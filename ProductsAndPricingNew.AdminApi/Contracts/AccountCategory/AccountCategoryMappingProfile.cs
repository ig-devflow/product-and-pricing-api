using AutoMapper;
using ProductsAndPricingNew.Application.Features.AccountCategory.Commands.CreateAccountCategory;
using ProductsAndPricingNew.Application.Features.AccountCategory.Commands.UpdateAccountCategory;

namespace ProductsAndPricingNew.AdminApi.Contracts.AccountCategory;

public sealed class AccountCategoryMappingProfile : Profile
{
    public AccountCategoryMappingProfile()
    {
        CreateMap<CreateAccountCategoryRequest, CreateAccountCategoryCommand>()
            .ConstructUsing(src => new CreateAccountCategoryCommand(0, src.Name, src.IsActive));

        CreateMap<UpdateAccountCategoryRequest, UpdateAccountCategoryCommand>()
            .ConstructUsing(src => new UpdateAccountCategoryCommand(0, src.Name, src.Version));
    }
}
