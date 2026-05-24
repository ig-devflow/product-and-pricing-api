namespace ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

/// <summary>
/// Reference-data source backing a field's value picker in the rule-builder UI. A null
/// value source on a descriptor means free input (number / text / date) or, for enum
/// fields, the descriptor's own enum values.
/// </summary>
public enum FieldValueSource
{
    Products = 1,
    Schools = 2,
    Agents = 3,
    Countries = 4,
    Divisions = 5,
    Currencies = 6
}
