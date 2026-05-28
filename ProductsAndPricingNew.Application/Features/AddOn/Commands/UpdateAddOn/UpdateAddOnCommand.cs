using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;
using ProductsAndPricingNew.Application.Features.AddOn.Abstractions;
using ProductsAndPricingNew.Domain.Entities.Products;

namespace ProductsAndPricingNew.Application.Features.AddOn.Commands.UpdateAddOn;

public sealed record UpdateAddOnCommand(
    int Id,
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
    DateOnly? ClosurePolicy,
    string Version
) : ICommand<Result<Unit>>, IAddOnCommandPayload;