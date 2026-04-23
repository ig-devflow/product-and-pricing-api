namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionAccreditationBanner(
    byte[] Data,
    string ContentType,
    string FileName
);