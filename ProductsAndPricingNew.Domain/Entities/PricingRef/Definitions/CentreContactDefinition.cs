using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;

public sealed record CentreContactDefinition(
    int ContactTypeId,
    string Name,
    string? Email,
    ImageFileDefinition? SignatureImage);