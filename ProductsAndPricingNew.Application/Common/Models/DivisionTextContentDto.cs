using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record DivisionTextContentDto(
    int Id,
    int ContentTemplateId,
    string ContentTemplateName,
    int? AudienceId,
    string? AudienceName,
    string Content,
    ContentFormat Format
);