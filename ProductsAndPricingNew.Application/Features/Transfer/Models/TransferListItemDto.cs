namespace ProductsAndPricingNew.Application.Features.Transfer.Models;

public sealed record TransferListItemDto(
    int Id,
    string DivisionName,
    string Name,
    bool IsActive,
    int TransferTypeId,
    int TransferPortId,
    DateOnly CreatedAt,
    string CreatedByName,
    DateOnly UpdatedAt,
    string UpdatedByName
);
