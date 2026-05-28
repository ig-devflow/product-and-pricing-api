namespace ProductsAndPricingNew.AdminApi.Contracts.Package;

public sealed record UpdatePackageRequest(
    string Name,
    int UnitTypeId,
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
    IReadOnlyCollection<PackageItemRequest> Items,
    string Version
);
