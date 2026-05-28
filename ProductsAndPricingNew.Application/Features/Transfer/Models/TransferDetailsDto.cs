namespace ProductsAndPricingNew.Application.Features.Transfer.Models;

public sealed record TransferDetailsDto(
    int Id,
    int DivisionId,
    int UnitTypeId,
    int TransferTypeId,
    int TransferPortId,
    TimeOnly? TimeFrom,
    TimeOnly? TimeTo,
    string Name,
    bool IsActive,
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
