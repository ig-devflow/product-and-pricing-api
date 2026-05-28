namespace ProductsAndPricingNew.AdminApi.Contracts.Transfer;

public sealed record CreateTransferRequest(
    string Name,
    int UnitTypeId,
    int TransferTypeId,
    int TransferPortId,
    TimeOnly? TimeFrom,
    TimeOnly? TimeTo,
    bool IsActive,
    int AccountCategoryId,
    int ProductCategoryId,
    string? GeneralLedgerCode,
    string? CostCentreCode,
    DateOnly? ClosurePolicy
);
