using ProductsAndPricingNew.Application.Common.Models;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreContactDto(
    CentreContactType ContactType,
    string Name,
    string? Email,
    ImageFileDto SignatureImage
);