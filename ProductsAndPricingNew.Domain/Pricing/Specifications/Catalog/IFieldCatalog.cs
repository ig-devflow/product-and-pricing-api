using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

/// <summary>
/// Registry of the fields rules may reference, grouped by subject. Consumed by the
/// rule-builder UI and by rule validation (the specification factory).
/// </summary>
public interface IFieldCatalog
{
    IReadOnlyList<FieldDescriptor> GetFields(RuleSubjectType subject);
    FieldDescriptor? Find(RuleSubjectType subject, string key);
}
