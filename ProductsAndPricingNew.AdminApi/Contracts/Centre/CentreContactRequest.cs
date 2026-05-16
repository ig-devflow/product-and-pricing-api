using ProductsAndPricingNew.AdminApi.Contracts.Common;
using ProductsAndPricingNew.Domain.Entities.PricingRef;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreContactRequest(
    CentreContactType ContactType,
    string Name,
    string? Email,
    ImageFileRequest SignatureImage
);