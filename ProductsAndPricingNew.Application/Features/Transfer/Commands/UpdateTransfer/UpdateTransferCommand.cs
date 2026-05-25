using FluentResults;
using MediatR;
using ProductsAndPricingNew.Application.Abstractions;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.UpdateTransfer;

public sealed record UpdateTransferCommand(
    int Id,
    int UnitTypeId,
    int TransferTypeId,
    int TransferPortId,
    TimeOnly? TimeFrom,
    TimeOnly? TimeTo,
    string Name,
    bool IsActive,
    int ProductCategoryId,
    int AccountCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy,
    string Version
) : ICommand<Result<Unit>>;