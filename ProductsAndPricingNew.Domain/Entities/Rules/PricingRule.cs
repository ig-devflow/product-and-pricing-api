using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.Entities.Rules;

public sealed class PricingRule : Entity<int>
{
    public int RulesetId { get; private set; }
    public string Name { get; private set; } = null!;
    public string ScriptJson { get; private set; } = null!;
    public int Priority { get; private set; }
    public bool IsActive { get; private set; }

    private PricingRule() { }

    // Id is database-generated; rules are identified within a ruleset by name.
    internal PricingRule(int rulesetId, string name, string scriptJson, int priority)
    {
        RulesetId = rulesetId;
        IsActive = true;

        ChangeName(name);
        ChangeScript(scriptJson);
        ChangePriority(priority);
    }

    internal void ChangeName(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), Rules.NameMaxLength);

    internal void ChangeScript(string scriptJson)
    {
        if (string.IsNullOrWhiteSpace(scriptJson))
            throw new DomainException("Rule script JSON is required.");

        ScriptJson = scriptJson.Trim();
    }

    internal void ChangePriority(int priority)
    {
        if (priority < 0)
            throw new DomainException("Priority cannot be negative.");

        Priority = priority;
    }

    internal void Activate() => IsActive = true;
    internal void Deactivate() => IsActive = false;

    public static class Rules
    {
        public const int NameMaxLength = 200;
    }
}
