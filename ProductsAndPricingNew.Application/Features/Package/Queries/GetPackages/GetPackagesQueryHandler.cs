using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Models;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Application.Features.Package.Queries.GetPackages;

internal sealed class GetPackagesQueryHandler : IRequestHandler<GetPackagesQuery, Result<PagedResult<PackageListItemDto>>>
{
    private readonly IPackageQuery _packageQuery;

    public GetPackagesQueryHandler(IPackageQuery packageQuery)
    {
        _packageQuery = packageQuery;
    }

    public async Task<Result<PagedResult<PackageListItemDto>>> Handle(GetPackagesQuery request, CancellationToken ct)
    {
        string? normalizedSearch = request.Search.AsOptionalText();

        PagedResult<PackageListItemDto> result = await _packageQuery.GetListAsync(
            request.DivisionId,
            normalizedSearch,
            request.IsActive,
            request.Paging,
            ct);

        return Result.Ok(result);
    }
}
