using ProductsAndPricingNew.Domain.ReferenceData;

namespace ProductsAndPricingNew.AdminApi.Contracts.Common;

public sealed record TextContentRequest(
    int ContentTemplateId,
    int? AudienceId,
    string? Content,
    ContentFormat Format
);