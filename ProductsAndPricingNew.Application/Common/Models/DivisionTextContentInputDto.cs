using ProductsAndPricingNew.Domain.Entities.ReferenceData;

namespace ProductsAndPricingNew.Application.Common.Models;

public sealed record DivisionTextContentInputDto(
    int ContentTemplateId,
    int? AudienceId,
    string? Content,
    ContentFormat Format
);