using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

/// <summary>
/// Describes one field rules of a given subject may reference. Drives the rule-builder UI
/// and validation of authored rules — the data-shaped replacement for the legacy
/// BasketFields constants plus IConstantValueType.
/// </summary>
public sealed record FieldDescriptor(
    RuleSubjectType Subject,
    string Key,
    RuleValueType DataType,
    string Label,
    IReadOnlyList<RuleOperator> AllowedOperators,
    FieldValueSource? ValueSource = null,
    IReadOnlyList<string>? EnumValues = null);
