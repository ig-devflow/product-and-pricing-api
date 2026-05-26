using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Accommodation.Commands.CreateAccommodation;
using ProductsAndPricingNew.Application.Features.Accommodation.Commands.UpdateAccommodation;
using ProductsAndPricingNew.Application.Features.Accommodation.Queries.GetAccommodations;

namespace ProductsAndPricingNew.AdminApi.Contracts.Accommodation;

public class AccommodationMappingProfile : Profile
{
    public AccommodationMappingProfile()
    {
        CreateMap<GetAccommodationsRequest, GetAccommodationsQuery>()
            .ConstructUsing(src => new GetAccommodationsQuery(
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateAccommodationRequest, CreateAccommodationCommand>()
            .ConstructUsing((src, ctx) => new CreateAccommodationCommand(
                src.Name,
                src.AccommodationTypeId,
                src.IsActive,
                src.MinimumStayInWeeks,
                src.AgeFrom,
                src.AgeTo,
                src.IsCommitted,
                src.IsNonCommitted));

        CreateMap<UpdateAccommodationRequest, UpdateAccommodationCommand>()
            .ConstructUsing(src => new UpdateAccommodationCommand(
                0,
                src.Name,
                src.AccommodationTypeId,
                src.IsActive,
                src.MinimumStayInWeeks,
                src.AgeFrom,
                src.AgeTo,
                src.IsCommitted,
                src.IsNonCommitted,
                src.Version));
    }
}