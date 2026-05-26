using ProductsAndPricingNew.AdminApi.Contracts.Common;

namespace ProductsAndPricingNew.AdminApi.Contracts.Centre;

public sealed record CentreContactRequest(
    int ContactTypeId,
    string Name,
    string? Email,
    ImageFileRequest SignatureImage
);