namespace ProductsAndPricingNew.Application.Features.Pricing.Models;

/// <summary>
/// A rule-builder field, as exposed to the UI. Enum-typed members of the domain descriptor
/// are projected to strings so the API contract stays stable.
/// </summary>
public sealed record FieldDescriptorDto(
    string Key,
    string DataType,
    string Label,
    IReadOnlyList<string> AllowedOperators,
    string? ValueSource,
    IReadOnlyList<string>? EnumValues
);
