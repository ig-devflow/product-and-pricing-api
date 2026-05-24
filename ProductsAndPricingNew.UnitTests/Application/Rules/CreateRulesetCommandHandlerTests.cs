using FluentResults;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Rules;
using ProductsAndPricingNew.Application.Features.Rules.Commands.CreateRuleset;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;
using ProductsAndPricingNew.UnitTests.TestSupport.Fakes;

namespace ProductsAndPricingNew.UnitTests.Application.Rules;

public sealed class CreateRulesetCommandHandlerTests
{
    private static RuleGroupModel ValidDefinition() =>
        new(RuleLogic.And, false, Conditions: [new RuleConditionModel("line.weeks", "Gte", Value: "2")]);

    private static RuleGroupModel InvalidDefinition() =>
        new(RuleLogic.And, false, Conditions: [new RuleConditionModel("no.such.field", "Eq", Value: "1")]);

    private static CreateRulesetCommand Command(params RulesetRuleModel[] rules) =>
        new("Summer discounts", "Seasonal offers", RuleSubjectType.ProductRow, DivisionId: 7, rules);

    private static CreateRulesetCommandHandler NewHandler(
        RulesetRepositoryFake? repository = null, UnitOfWorkFake? unitOfWork = null) =>
        new(repository ?? new RulesetRepositoryFake(),
            new RuleSpecificationFactory(new FieldCatalog()),
            unitOfWork ?? new UnitOfWorkFake());

    [Fact]
    public async Task Handle_ValidCommand_CreatesRulesetWithRulesAndPersists()
    {
        RulesetRepositoryFake repository = new();
        UnitOfWorkFake unitOfWork = new();
        CreateRulesetCommandHandler handler = NewHandler(repository, unitOfWork);

        Result<int> result = await handler.Handle(
            Command(
                new RulesetRuleModel("Early bird", 10, ValidDefinition()),
                new RulesetRuleModel("Loyalty", 20, ValidDefinition())),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, repository.AddCalls);
        Assert.Equal(1, unitOfWork.SaveChangesCalls);
        Assert.NotNull(repository.AddedRuleset);
        Assert.Equal(RuleSubjectType.ProductRow, repository.AddedRuleset.SubjectType);
        Assert.Equal(2, repository.AddedRuleset.Rules.Count);
    }

    [Fact]
    public async Task Handle_StoresEachRuleAsValidSpecificationJson()
    {
        RulesetRepositoryFake repository = new();
        CreateRulesetCommandHandler handler = NewHandler(repository);

        await handler.Handle(
            Command(new RulesetRuleModel("Early bird", 10, ValidDefinition())),
            CancellationToken.None);

        PricingRule rule = Assert.Single(repository.AddedRuleset!.Rules);
        RuleNode parsed = RuleSpecJson.Deserialize(rule.ScriptJson);
        Assert.NotNull(parsed);
    }

    [Fact]
    public async Task Handle_InvalidRuleDefinition_ReturnsValidationErrorAndDoesNotPersist()
    {
        RulesetRepositoryFake repository = new();
        UnitOfWorkFake unitOfWork = new();
        CreateRulesetCommandHandler handler = NewHandler(repository, unitOfWork);

        Result<int> result = await handler.Handle(
            Command(new RulesetRuleModel("Broken", 10, InvalidDefinition())),
            CancellationToken.None);

        Assert.True(result.IsFailed);
        Assert.IsType<ValidationError>(Assert.Single(result.Errors));
        Assert.Equal(0, repository.AddCalls);
        Assert.Equal(0, unitOfWork.SaveChangesCalls);
    }
}
