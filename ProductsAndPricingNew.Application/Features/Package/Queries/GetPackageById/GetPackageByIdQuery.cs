using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Queries.GetPackageById;

public sealed record GetPackageByIdQuery(int Id) : IRequest<Result<PackageDetailsDto>>;
