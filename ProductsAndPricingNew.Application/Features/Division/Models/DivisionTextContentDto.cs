using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.Application.Features.Division.Models;

public sealed record DivisionTextContentDto(
    int Id,
    int ContentTemplateId,
    string ContentTemplateName,
    int? AudienceId,
    string? AudienceName,
    string Content,
    ContentFormat Format
);