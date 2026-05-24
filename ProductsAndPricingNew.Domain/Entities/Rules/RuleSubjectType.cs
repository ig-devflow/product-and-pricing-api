namespace ProductsAndPricingNew.Domain.Entities.Rules;

/// <summary>
/// What a rule's specification is evaluated against. Bound to a CLR fact type in the
/// pricing engine. The rule "purpose" (apply / applicable-row / charge) is not encoded
/// here — it is implied by which property holds the <c>RulesetRef</c>.
/// </summary>
public enum RuleSubjectType
{
    ProductRow = 1,
    Booking = 2,
    Cancellation = 3
}
