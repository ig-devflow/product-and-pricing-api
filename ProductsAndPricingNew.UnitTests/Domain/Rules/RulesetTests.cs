using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.UnitTests.Domain.Rules;

public sealed class RulesetTests
{
    private const string Script = "{\"op\":\"true\"}";

    private static Ruleset NewRuleset() =>
        Ruleset.Create("Discounts", RuleSubjectType.ProductRow, divisionId: 7);

    [Fact]
    public void Create_SetsDefaults()
    {
        Ruleset ruleset = NewRuleset();

        Assert.Equal("Discounts", ruleset.Name);
        Assert.Equal(RuleSubjectType.ProductRow, ruleset.SubjectType);
        Assert.Equal(7, ruleset.DivisionId);
        Assert.Equal(RuleStatus.Draft, ruleset.Status);
        Assert.Equal(1, ruleset.RuleVersion);
    }

    [Fact]
    public void AddRule_AddsRuleAndBumpsVersion()
    {
        Ruleset ruleset = NewRuleset();

        PricingRule rule = ruleset.AddRule("Early bird", Script, priority: 5);

        Assert.Single(ruleset.Rules);
        Assert.Equal("Early bird", rule.Name);
        Assert.Equal(2, ruleset.RuleVersion);
    }

    [Fact]
    public void AddRule_DuplicateName_Throws()
    {
        Ruleset ruleset = NewRuleset();
        ruleset.AddRule("Early bird", Script, 1);

        // Names are matched case-insensitively.
        Assert.Throws<DomainException>(() => ruleset.AddRule("early bird", Script, 2));
    }

    [Fact]
    public void UpdateRule_BumpsVersion()
    {
        Ruleset ruleset = NewRuleset();
        ruleset.AddRule("Early bird", Script, 1);

        ruleset.UpdateRule("Early bird", Script, 2);

        Assert.Equal(3, ruleset.RuleVersion);
    }

    [Fact]
    public void UpdateRule_NotFound_Throws()
    {
        Ruleset ruleset = NewRuleset();

        Assert.Throws<DomainException>(() => ruleset.UpdateRule("Ghost", Script, 1));
    }

    [Fact]
    public void RemoveRule_RemovesRuleAndBumpsVersion()
    {
        Ruleset ruleset = NewRuleset();
        ruleset.AddRule("Early bird", Script, 1);

        ruleset.RemoveRule("Early bird");

        Assert.Empty(ruleset.Rules);
        Assert.Equal(3, ruleset.RuleVersion);
    }

    [Fact]
    public void RemoveRule_NotFound_Throws()
    {
        Ruleset ruleset = NewRuleset();

        Assert.Throws<DomainException>(() => ruleset.RemoveRule("Ghost"));
    }

    [Fact]
    public void Publish_DoesNotBumpRuleVersion()
    {
        Ruleset ruleset = NewRuleset();
        ruleset.AddRule("Early bird", Script, 1); // RuleVersion -> 2

        ruleset.Publish();

        Assert.Equal(RuleStatus.Published, ruleset.Status);
        Assert.Equal(2, ruleset.RuleVersion);
    }

    [Fact]
    public void Publish_WhenArchived_Throws()
    {
        Ruleset ruleset = NewRuleset();
        ruleset.Archive();

        Assert.Throws<DomainException>(ruleset.Publish);
    }
}
