using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Commands.UpdatePackage;

public sealed record UpdatePackageCommand(
    int Id,
    int UnitTypeId,
    string Name,
    bool IsActive,
    string? Description,
    decimal Commission,
    int? AgeFrom,
    int? AgeTo,
    int? MinimumWeeks,
    int ProductCategoryId,
    int AccountCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    IReadOnlyCollection<PackageItemDto> PackageItems,
    string Version
) : ICommand<Result<Unit>>, IPackageCommandPayload
{
    IReadOnlyCollection<PackageItemDto> IPackageCommandPayload.Items => PackageItems;
}