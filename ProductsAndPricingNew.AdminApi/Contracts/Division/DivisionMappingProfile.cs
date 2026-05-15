using AutoMapper;
using ProductsAndPricingNew.AdminApi.Contracts.Common;
using ProductsAndPricingNew.Application.Common.Models;
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
            .ConstructUsing((src, ctx) => new CreateDivisionCommand(
                src.Name,
                src.WebsiteUrl,
                src.IsActive,
                src.TermsAndConditions,
                src.GroupsPaymentTerms,
                src.HeadOfficeEmail,
                src.HeadOfficeTelephoneNo,
                ctx.Mapper.Map<AddressDto?>(src.ContactAddress),
                ctx.Mapper.Map<ImageFileDto?>(src.AccreditationBanner),
                ctx.Mapper.Map<IReadOnlyCollection<TextContentDto>>(src.Texts)));

        CreateMap<UpdateDivisionRequest, UpdateDivisionCommand>()
            .ConstructUsing((src, ctx) => new UpdateDivisionCommand(
                0,
                src.Name,
                src.WebsiteUrl,
                src.IsActive,
                src.TermsAndConditions,
                src.GroupsPaymentTerms,
                src.HeadOfficeEmail,
                src.HeadOfficeTelephoneNo,
                ctx.Mapper.Map<AddressDto?>(src.ContactAddress),
                ctx.Mapper.Map<ImageFileDto?>(src.AccreditationBanner),
                ctx.Mapper.Map<IReadOnlyCollection<TextContentDto>>(src.Texts),
                src.Version));
    }
}