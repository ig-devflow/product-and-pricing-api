namespace ProductsAndPricingNew.Application.Features.Rules.Models;

/// <summary>
/// One UI-authored condition. <see cref="Value"/> is used by single-value operators
/// (Eq/NotEq/Lt/Lte/Gt/Gte); <see cref="Values"/> by In/NotIn; HasValue uses neither.
/// Values are carried as strings and parsed by the factory against the field's data type.
/// </summary>
public sealed record RuleConditionModel(
    string Field,
    string Operator,
    string? Value = null,
    IReadOnlyList<string>? Values = null);
