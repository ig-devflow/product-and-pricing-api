using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Common.Primitives;
using ProductsAndPricingNew.Domain.Common.Text;

namespace ProductsAndPricingNew.Domain.Entities.Rules;

public sealed class Ruleset : AggregateRoot<int>
{
    private readonly List<PricingRule> _rules = new();

    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public RuleSubjectType SubjectType { get; private set; }
    public int DivisionId { get; private set; }
    public RuleStatus Status { get; private set; }

    /// <summary>
    /// Content version — incremented whenever the contained rules change. The pricing engine
    /// uses it to invalidate its compiled NRules session cache.
    /// </summary>
    public int RuleVersion { get; private set; }

    public IReadOnlyCollection<PricingRule> Rules => _rules.AsReadOnly();

    private Ruleset() { }

    public static Ruleset Create(string name, RuleSubjectType subjectType, int divisionId)
    {
        Ruleset ruleset = new()
        {
            SubjectType = subjectType,
            DivisionId = divisionId,
            Status = RuleStatus.Draft,
            RuleVersion = 1
        };

        ruleset.Rename(name);

        return ruleset;
    }

    public void Rename(string name) =>
        Name = name.AsRequiredDomainText(nameof(Name), EntityRules.NameMaxLength);

    public void ChangeDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    public void Publish()
    {
        if (Status == RuleStatus.Archived)
            throw new DomainException("Cannot publish an archived ruleset.");

        Status = RuleStatus.Published;
    }

    public void Archive() => Status = RuleStatus.Archived;

    /// <summary>
    /// Adds a new rule. The rule's database id is assigned on save; rules are identified
    /// within a ruleset by their (case-insensitive, unique) name.
    /// </summary>
    public PricingRule AddRule(string ruleName, string ruleScriptJson, int priority)
    {
        PricingRule rule = new(Id, ruleName, ruleScriptJson, priority);

        if (_rules.Any(existing => NameEquals(existing.Name, rule.Name)))
            throw new DomainException($"A rule named '{rule.Name}' already exists in this ruleset.");

        _rules.Add(rule);
        BumpVersion();

        return rule;
    }

    public void UpdateRule(string ruleName, string ruleScriptJson, int priority)
    {
        PricingRule rule = GetRule(ruleName);
        rule.ChangeScript(ruleScriptJson);
        rule.ChangePriority(priority);
        BumpVersion();
    }

    public void RemoveRule(string ruleName)
    {
        PricingRule rule = GetRule(ruleName);
        _rules.Remove(rule);
        BumpVersion();
    }

    private PricingRule GetRule(string ruleName)
    {
        string target = (ruleName ?? string.Empty).Trim();

        return _rules.FirstOrDefault(rule => NameEquals(rule.Name, target))
               ?? throw new DomainException($"Rule '{ruleName}' was not found in ruleset {Id}.");
    }

    private static bool NameEquals(string left, string right) =>
        string.Equals(left, right, StringComparison.OrdinalIgnoreCase);

    private void BumpVersion() => RuleVersion++;

    public static class EntityRules
    {
        public const int NameMaxLength = 200;
        public const int DescriptionMaxLength = 1000;
    }
}
