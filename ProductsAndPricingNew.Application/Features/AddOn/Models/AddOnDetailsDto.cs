using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.AddOn.Models;

public sealed record AddOnDetailsDto(
    int Id,
    int DivisionId,
    int UnitTypeId,
    AddOnType AddOnType,
    string Name,
    bool IsActive,
    int? AgeFrom,
    int? AgeTo,
    int? OneToOneLessonsPerWeek,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
