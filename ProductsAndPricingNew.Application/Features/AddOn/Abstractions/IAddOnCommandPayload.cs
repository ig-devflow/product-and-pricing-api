using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.AddOn.Abstractions;

internal interface IAddOnCommandPayload
{
    int UnitTypeId { get; }
    AddOnType AddOnType { get; }
    string Name { get; }
    bool IsActive { get; }
    int AccountCategoryId { get; }
    int ProductCategoryId { get; }
    int? AgeFrom { get; }
    int? AgeTo { get; }
    int? OneToOneLessonsPerWeek { get; }
    string? GeneralLedgerCode { get; }
    string? CostCentreCode { get; }
    DateOnly? ClosurePolicy { get; }
}
