using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Queries.GetPackageById;

internal sealed class GetPackageByIdQueryHandler : IRequestHandler<GetPackageByIdQuery, Result<PackageDetailsDto>>
{
    private readonly IPackageQuery _packageQuery;

    public GetPackageByIdQueryHandler(IPackageQuery packageQuery)
    {
        _packageQuery = packageQuery;
    }

    public async Task<Result<PackageDetailsDto>> Handle(GetPackageByIdQuery request, CancellationToken ct)
    {
        PackageDetailsDto? result = await _packageQuery.GetByIdAsync(request.Id, ct);
        if (result is null)
            return Result.Fail(new NotFoundError($"Package with id {request.Id} was not found"));

        return Result.Ok(result);
    }
}
