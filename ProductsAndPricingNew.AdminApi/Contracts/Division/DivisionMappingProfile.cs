using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Division.Commands.CreateDivision;
using ProductsAndPricingNew.Application.Features.Division.Commands.UpdateDivision;
using ProductsAndPricingNew.Application.Features.Division.Queries.GetDivisions;

namespace ProductsAndPricingNew.AdminApi.Contracts.Division;

public sealed class DivisionMappingProfile : Profile
{
    public DivisionMappingProfile()
    {
        CreateMap<GetDivisionsRequest, GetDivisionsQuery>()
            .ConstructUsing(src => new GetDivisionsQuery(
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateDivisionRequest, CreateDivisionCommand>()
            .ConstructUsing(src => new CreateDivisionCommand(
                src.Name,
                src.WebsiteUrl,
                src.IsActive,
                src.TermsAndConditions,
                src.GroupsPaymentTerms,
                src.HeadOfficeEmail,
                src.HeadOfficeTelephoneNo,
                src.ContactAddress,
                src.AccreditationBanner,
                src.Texts));

        CreateMap<UpdateDivisionRequest, UpdateDivisionCommand>()
            .ConstructUsing(src => new UpdateDivisionCommand(
                0,
                src.Name,
                src.WebsiteUrl,
                src.IsActive,
                src.TermsAndConditions,
                src.GroupsPaymentTerms,
                src.HeadOfficeEmail,
                src.HeadOfficeTelephoneNo,
                src.ContactAddress,
                src.AccreditationBanner,
                src.Texts));
    }
}