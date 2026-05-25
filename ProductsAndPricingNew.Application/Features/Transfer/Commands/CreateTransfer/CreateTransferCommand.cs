using FluentResults;
using MediatR;

namespace ProductsAndPricingNew.Application.Features.Transfer.Commands.CreateTransfer;

public sealed record CreateTransferCommand(
    int DivisionId,
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
    DateOnly? ClosurePolicy
) : IRequest<Result<int>>;