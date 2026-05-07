using FluentResults;

namespace ProductsAndPricingNew.Application.Common.Errors;

public sealed class DomainRuleViolationError : Error
{
    public const string Code = "domain_rule_violation";

    public DomainRuleViolationError(string message) : base(message)
    {
        Metadata.Add("Code", Code);
    }
}