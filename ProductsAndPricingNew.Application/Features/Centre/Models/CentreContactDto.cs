using ProductsAndPricingNew.Application.Common.Models;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreContactDto(
    int ContactTypeId,
    string Name,
    string? Email,
    ImageFileDto SignatureImage
);