namespace ProductsAndPricingNew.AdminApi.Contracts.Common;

public sealed record ImageFileRequest(
    byte[]? Data,
    string? ContentType,
    string? FileName
);