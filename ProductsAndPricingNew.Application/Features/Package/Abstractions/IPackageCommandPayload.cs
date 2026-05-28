using ProductsAndPricingNew.Application.Features.Package.Models;

namespace ProductsAndPricingNew.Application.Features.Package.Abstractions;

internal interface IPackageCommandPayload
{
    int UnitTypeId { get; }
    string Name { get; }
    bool IsActive { get; }
    string? Description { get; }
    decimal Commission { get; }
    int? AgeFrom { get; }
    int? AgeTo { get; }
    int? MinimumWeeks { get; }
    int AccountCategoryId { get; }
    int ProductCategoryId { get; }
    string? GeneralLedgerCode { get; }
    string? CostCentreCode { get; }
    DateOnly? ClosurePolicy { get; }
    IReadOnlyCollection<PackageItemDto> Items { get; }
}
