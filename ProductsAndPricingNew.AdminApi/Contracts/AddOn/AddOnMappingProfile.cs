using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AddOn.Commands.CreateAddOn;
using ProductsAndPricingNew.Application.Features.AddOn.Commands.UpdateAddOn;
using ProductsAndPricingNew.Application.Features.AddOn.Queries.GetAddOns;

namespace ProductsAndPricingNew.AdminApi.Contracts.AddOn;

public class AddOnMappingProfile : Profile
{
    public AddOnMappingProfile()
    {
        CreateMap<GetAddOnsRequest, GetAddOnsQuery>()
            .ConstructUsing(src => new GetAddOnsQuery(
                0,
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateAddOnRequest, CreateAddOnCommand>()
            .ConstructUsing(src => new CreateAddOnCommand(
                0,
                src.UnitTypeId,
                src.AddOnType,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.AgeFrom,
                src.AgeTo,
                src.OneToOneLessonsPerWeek,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy));

        CreateMap<UpdateAddOnRequest, UpdateAddOnCommand>()
            .ConstructUsing(src => new UpdateAddOnCommand(
                0,
                src.UnitTypeId,
                src.AddOnType,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.AgeFrom,
                src.AgeTo,
                src.OneToOneLessonsPerWeek,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                src.Version));
    }
}
