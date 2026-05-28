namespace ProductsAndPricingNew.Application.Features.Transfer.Abstractions;

internal interface ITransferCommandPayload
{
    int UnitTypeId { get; }
    int TransferTypeId { get; }
    int TransferPortId { get; }
    TimeOnly? TimeFrom { get; }
    TimeOnly? TimeTo { get; }
    string Name { get; }
    bool IsActive { get; }
    int AccountCategoryId { get; }
    int ProductCategoryId { get; }
    string? GeneralLedgerCode { get; }
    string? CostCentreCode { get; }
    DateOnly? ClosurePolicy { get; }
}
