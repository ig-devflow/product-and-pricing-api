using ProductsAndPricingNew.Domain.SharedKernel.Definitions;

namespace ProductsAndPricingNew.Domain.Entities.PricingRef.Definitions;

public sealed record CentreContactDefinition(
    CentreContactType ContactType,
    string Name,
    string? Email,
    ImageFileDefinition? SignatureImage);