namespace ProductsAndPricingNew.AdminApi.Contracts.Common;

public sealed record ImageBannerRequest(
    byte[]? Data,
    string? ContentType,
    string? FileName
);