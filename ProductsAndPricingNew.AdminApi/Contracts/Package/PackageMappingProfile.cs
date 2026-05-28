using AutoMapper;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Commands.CreatePackage;
using ProductsAndPricingNew.Application.Features.Package.Commands.UpdatePackage;
using ProductsAndPricingNew.Application.Features.Package.Models;
using ProductsAndPricingNew.Application.Features.Package.Queries.GetPackages;

namespace ProductsAndPricingNew.AdminApi.Contracts.Package;

public class PackageMappingProfile : Profile
{
    public PackageMappingProfile()
    {
        CreateMap<GetPackagesRequest, GetPackagesQuery>()
            .ConstructUsing(src => new GetPackagesQuery(
                0,
                src.Search,
                src.IsActive,
                new PagingFilter(src.Page, src.PageSize)));

        CreateMap<PackageItemRequest, PackageItemDto>();

        CreateMap<CreatePackageRequest, CreatePackageCommand>()
            .ConstructUsing((src, ctx) => new CreatePackageCommand(
                0,
                src.UnitTypeId,
                src.Name,
                src.IsActive,
                src.Description,
                src.Commission,
                src.AgeFrom,
                src.AgeTo,
                src.MinimumWeeks,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                ctx.Mapper.Map<IReadOnlyCollection<PackageItemDto>>(src.Items)));

        CreateMap<UpdatePackageRequest, UpdatePackageCommand>()
            .ConstructUsing((src, ctx) => new UpdatePackageCommand(
                0,
                src.UnitTypeId,
                src.Name,
                src.IsActive,
                src.Description,
                src.Commission,
                src.AgeFrom,
                src.AgeTo,
                src.MinimumWeeks,
                src.ProductCategoryId,
                src.AccountCategoryId,
                src.GeneralLedgerCode,
                src.CostCentreCode,
                src.ClosurePolicy,
                ctx.Mapper.Map<IReadOnlyCollection<PackageItemDto>>(src.Items),
                src.Version));
    }
}
