namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record ImageBannerDto(
    byte[]? Data,
    string? ContentType,
    string? FileName
);