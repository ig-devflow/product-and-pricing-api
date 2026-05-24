using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

/// <summary>
/// A flat bag of fields the specification AST is evaluated against — the modern replacement
/// for the legacy <c>IFields</c>. Keys come from the field catalog (e.g. "product.kind",
/// "line.weeks"); values are boxed CLR values, or absent/null when unknown.
/// </summary>
public sealed record RuleFact(RuleSubjectType Subject, IReadOnlyDictionary<string, object?> Fields)
{
    public object? Get(string field) =>
        Fields.GetValueOrDefault(field);

    public bool Has(string field) => Get(field) is not null;
}
