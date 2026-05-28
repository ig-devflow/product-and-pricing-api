namespace ProductsAndPricingNew.Application.Features.Package.Models;

public sealed record PackageDetailsDto(
    int Id,
    int DivisionId,
    int UnitTypeId,
    string Name,
    bool IsActive,
    string? Description,
    decimal Commission,
    int? AgeFrom,
    int? AgeTo,
    int? MinimumWeeks,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    IReadOnlyCollection<PackageItemDto> Items,
    string Version,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
