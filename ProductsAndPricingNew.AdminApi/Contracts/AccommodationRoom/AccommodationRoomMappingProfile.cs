using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.CreateAccommodationRoom;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Commands.UpdateAccommodationRoom;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Models;
using ProductsAndPricingNew.Application.Features.AccommodationRoom.Queries.GetAccommodationRooms;

namespace ProductsAndPricingNew.AdminApi.Contracts.AccommodationRoom;

public class AccommodationRoomMappingProfile : Profile
{
    public AccommodationRoomMappingProfile()
    {
        CreateMap<GetAccommodationRoomsRequest, GetAccommodationRoomsQuery>()
            .ConstructUsing(src => new GetAccommodationRoomsQuery(
                0,
                0,
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateAccommodationRoomRequest, CreateAccommodationRoomCommand>()
            .ConstructUsing((src, ctx) => new CreateAccommodationRoomCommand(
                0,
                0,
                src.Name,
                src.UnitTypeId,
                src.IsActive,
                src.OccupyRoom,
                ctx.Mapper.Map<RoomDetailsDto>(src.RoomDetails),
                src.AccountCategoryId,
                src.ProductCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy));

        CreateMap<UpdateAccommodationRoomRequest, UpdateAccommodationRoomCommand>()
            .ConstructUsing((src, ctx) => new UpdateAccommodationRoomCommand(
                0,
                src.Name,
                src.UnitTypeId,
                src.IsActive,
                src.OccupyRoom,
                ctx.Mapper.Map<RoomDetailsDto>(src.RoomDetails),
                src.AccountCategoryId,
                src.ProductCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                src.Version));

        CreateMap<RoomDetailsRequest, RoomDetailsDto>();
    }
}