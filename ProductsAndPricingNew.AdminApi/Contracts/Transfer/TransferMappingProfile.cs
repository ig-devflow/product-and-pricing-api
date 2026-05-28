using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Transfer.Commands.CreateTransfer;
using ProductsAndPricingNew.Application.Features.Transfer.Commands.UpdateTransfer;
using ProductsAndPricingNew.Application.Features.Transfer.Queries.GetTransfers;

namespace ProductsAndPricingNew.AdminApi.Contracts.Transfer;

public class TransferMappingProfile : Profile
{
    public TransferMappingProfile()
    {
        CreateMap<GetTransfersRequest, GetTransfersQuery>()
            .ConstructUsing(src => new GetTransfersQuery(
                0,
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<CreateTransferRequest, CreateTransferCommand>()
            .ConstructUsing(src => new CreateTransferCommand(
                0,
                src.UnitTypeId,
                src.TransferTypeId,
                src.TransferPortId,
                src.TimeFrom,
                src.TimeTo,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy));

        CreateMap<UpdateTransferRequest, UpdateTransferCommand>()
            .ConstructUsing(src => new UpdateTransferCommand(
                0,
                src.UnitTypeId,
                src.TransferTypeId,
                src.TransferPortId,
                src.TimeFrom,
                src.TimeTo,
                src.Name,
                src.IsActive,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                src.Version));
    }
}
