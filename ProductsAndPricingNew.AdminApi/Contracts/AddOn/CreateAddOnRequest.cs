using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.AdminApi.Contracts.AddOn;

public sealed record CreateAddOnRequest(
    string Name,
    int UnitTypeId,
    AddOnType AddOnType,
    bool IsActive,
    int AccountCategoryId,
    int ProductCategoryId,
    int? AgeFrom,
    int? AgeTo,
    int? OneToOneLessonsPerWeek,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
);
