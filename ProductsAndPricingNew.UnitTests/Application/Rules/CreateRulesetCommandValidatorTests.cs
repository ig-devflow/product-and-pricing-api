using ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;

namespace ProductsAndPricingNew.UnitTests.Application.Rules;

public sealed class CreateRulesetCommandValidatorTests
{
    private readonly CreateRulesetCommandValidator _validator = new();

    private static RuleGroupModel Definition() =>
        new(RuleLogic.And, false, Conditions: [new RuleConditionModel("line.weeks", "Gte", Value: "2")]);

    private static CreateRulesetCommand Command(
        IReadOnlyList<RulesetRuleModel> rules, string name = "Discounts", int divisionId = 7) =>
        new(name, null, RuleSubjectType.ProductRow, divisionId, rules);

    [Fact]
    public void Validate_ValidCommand_Passes()
    {
        CreateRulesetCommand command = Command([new RulesetRuleModel("Early bird", 10, Definition())]);

        Assert.True(_validator.Validate(command).IsValid);
    }

    [Fact]
    public void Validate_EmptyName_Fails()
    {
        CreateRulesetCommand command = Command([new RulesetRuleModel("Early bird", 10, Definition())], name: "  ");

        Assert.False(_validator.Validate(command).IsValid);
    }

    [Fact]
    public void Validate_InvalidDivision_Fails()
    {
        CreateRulesetCommand command = Command([new RulesetRuleModel("Early bird", 10, Definition())], divisionId: 0);

        Assert.False(_validator.Validate(command).IsValid);
    }

    [Fact]
    public void Validate_DuplicateRuleNames_Fails()
    {
        CreateRulesetCommand command = Command(
        [
            new RulesetRuleModel("Early bird", 10, Definition()),
            new RulesetRuleModel("early bird", 20, Definition())
        ]);

        Assert.False(_validator.Validate(command).IsValid);
    }

    [Fact]
    public void Validate_NegativeRulePriority_Fails()
    {
        CreateRulesetCommand command = Command([new RulesetRuleModel("Early bird", -1, Definition())]);

        Assert.False(_validator.Validate(command).IsValid);
    }

    [Fact]
    public void Validate_EmptyRulesCollection_Passes()
    {
        CreateRulesetCommand command = Command([]);

        Assert.True(_validator.Validate(command).IsValid);
    }
}
