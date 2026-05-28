using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Pagination;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Queries.GetPackages;

public sealed record GetPackagesQuery(
    int DivisionId,
    string? Search = null,
    bool? IsActive = null,
    PagingFilter Paging = default
) : IRequest<Result<PagedResult<PackageListItemDto>>>;
