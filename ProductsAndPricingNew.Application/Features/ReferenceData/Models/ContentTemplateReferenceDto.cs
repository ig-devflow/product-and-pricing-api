using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.ReferenceData.Models;

public sealed record ContentTemplateReferenceDto(
    int Id,
    string Name,
    string? Description,
    ContentTemplateScope Scope
);
