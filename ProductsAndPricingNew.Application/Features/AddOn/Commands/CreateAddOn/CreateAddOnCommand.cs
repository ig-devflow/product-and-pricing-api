using FluentResults;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.CreateAddOn;

public sealed record CreateAddOnCommand(
    int DivisionId,
    int UnitTypeId,
    AddOnType AddOnType,
    string Name,
    bool IsActive,
    int ProductCategoryId,
    int AccountCategoryId,
    int? AgeFrom,
    int? AgeTo,
    int? OneToOneLessonsPerWeek,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
) : ICommand<Result<int>>, IAddOnCommandPayload;