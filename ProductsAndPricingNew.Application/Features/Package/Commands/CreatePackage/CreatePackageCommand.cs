using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Abstractions;
using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Commands.CreatePackage;

public sealed record CreatePackageCommand(
    int DivisionId,
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
    IReadOnlyCollection<PackageItemDto> Items
) : ICommand<Result<int>>, IPackageCommandPayload;