using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.Centre.Models;

public sealed record CentreTextContentDto(
    int Id,
    int ContentTemplateId,
    string ContentTemplateName,
    int? AudienceId,
    string? AudienceName,
    string Content,
    ContentFormat Format
);