namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record ImageFileDto(
    byte[]? Data,
    string? ContentType,
    string? FileName
);