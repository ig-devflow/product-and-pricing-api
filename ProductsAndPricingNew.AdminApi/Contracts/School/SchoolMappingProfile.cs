using AutoMapper;
using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.School.Commands.CreateSchool;
using ProductsAndPricingNew.Application.Features.School.Commands.UpdateSchool;
using ProductsAndPricingNew.Application.Features.School.Queries.GetSchools;

namespace ProductsAndPricingNew.AdminApi.Contracts.School;

public sealed class SchoolMappingProfile : Profile
{
    public SchoolMappingProfile()
    {
        CreateMap<GetSchoolsRequest, GetSchoolsQuery>()
            .ConstructUsing(src => new GetSchoolsQuery(
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize),
                null));

        CreateMap<CreateSchoolRequest, CreateSchoolCommand>()
            .ConstructUsing((src, ctx) => new CreateSchoolCommand(
                0,
                src.Name,
                src.LegacyCode,
                src.MinimumStayInWeeks,
                src.AgeFrom,
                src.AgeTo,
                src.Telephone,
                src.EmergencyTelephone,
                ctx.Mapper.Map<AddressDto?>(src.ContactAddress),
                src.FinanceCode,
                src.LmsAccess,
                src.IsActive,
                src.DecommissionDate));

        CreateMap<UpdateSchoolRequest, UpdateSchoolCommand>()
            .ConstructUsing(src => new UpdateSchoolCommand(
                0,
                src.Name,
                src.LegacyCode,
                src.MinimumStayInWeeks,
                src.AgeFrom,
                src.AgeTo,
                src.Telephone,
                src.EmergencyTelephone,
                src.ContactAddress,
                src.FinanceCode,
                src.LmsAccess,
                src.IsActive,
                src.DecommissionDate,
                src.Version));
    }
}