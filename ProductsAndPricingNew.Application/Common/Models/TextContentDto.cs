using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record TextContentDto(
    int ContentTemplateId,
    int? AudienceId,
    string? Content,
    ContentFormat Format
);