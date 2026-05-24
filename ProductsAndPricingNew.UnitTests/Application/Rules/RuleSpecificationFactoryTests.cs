using FluentResults;
using ProductsAndPricingNew.Application.Common.Errors;
using ProductsAndPricingNew.Application.Features.Rules;
using ProductsAndPricingNew.Application.Features.Rules.Models;
using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

namespace ProductsAndPricingNew.UnitTests.Application.Rules;

public sealed class RuleSpecificationFactoryTests
{
    private readonly RuleSpecificationFactory _factory = new(new FieldCatalog());

    private static RuleConditionModel Cmp(string field, string op, string value) => new(field, op, Value: value);

    private static RuleConditionModel In(string field, string op, params string[] values) => new(field, op, Values: values);

    private static RuleFact ProductLine(ProductKind kind, int productId, string country, int agentId, DateOnly booked) =>
        new(RuleSubjectType.ProductRow, new Dictionary<string, object?>
        {
            ["booking.dateBooked"] = booked,
            ["product.kind"] = kind,
            ["product.id"] = productId,
            ["line.saleCountry"] = country,
            ["line.agentId"] = agentId
        });

    [Fact]
    public void Build_DiscountExample_ProducesAstThatEvaluatesCorrectly()
    {
        RuleGroupModel model = new(RuleLogic.And, Negate: false,
            Conditions: [Cmp("booking.dateBooked", "Lte", "2026-05-11")],
            Groups:
            [
                new RuleGroupModel(RuleLogic.Or, false, Groups:
                [
                    new RuleGroupModel(RuleLogic.And, false, Conditions:
                        [Cmp("product.kind", "Eq", "Course"), In("product.id", "In", "7", "15", "76", "81")]),
                    new RuleGroupModel(RuleLogic.And, false, Conditions:
                        [Cmp("product.kind", "Eq", "Transfer"), In("product.id", "NotIn", "6", "9", "55")])
                ]),
                new RuleGroupModel(RuleLogic.And, false, Conditions: [In("line.saleCountry", "In", "EG", "MT")]),
                new RuleGroupModel(RuleLogic.And, Negate: true, Conditions: [In("line.agentId", "In", "17", "8", "102", "104")])
            ]);

        Result<RuleNode> result = _factory.Build(RuleSubjectType.ProductRow, model);

        Assert.True(result.IsSuccess);
        RuleNode spec = result.Value;
        Assert.True(spec.IsSatisfiedBy(ProductLine(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 5, 1))));
        Assert.True(spec.IsSatisfiedBy(ProductLine(ProductKind.Transfer, 42, "MT", 5, new DateOnly(2026, 5, 11))));
        Assert.False(spec.IsSatisfiedBy(ProductLine(ProductKind.Course, 99, "EG", 5, new DateOnly(2026, 5, 1))));
        Assert.False(spec.IsSatisfiedBy(ProductLine(ProductKind.Transfer, 9, "EG", 5, new DateOnly(2026, 5, 1))));
        Assert.False(spec.IsSatisfiedBy(ProductLine(ProductKind.Course, 15, "EG", 102, new DateOnly(2026, 5, 1))));
        Assert.False(spec.IsSatisfiedBy(ProductLine(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 6, 1))));
    }

    [Fact]
    public void Build_UnknownField_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("no.such.field", "Eq", "1")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_OperatorNotAllowedForField_Fails()
    {
        // product.kind is an enum field — ordering operators are not allowed.
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("product.kind", "Lt", "Course")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_UnknownOperator_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("line.weeks", "Approximately", "2")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_NonNumericValueForIntField_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("line.weeks", "Gte", "two")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_InvalidDateValue_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("booking.dateBooked", "Lte", "11-05-2026")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_EnumValueNotAnOption_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [Cmp("product.kind", "Eq", "Spaceship")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_InWithNoValues_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [In("product.id", "In")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_ComparisonWithoutValue_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [new RuleConditionModel("line.weeks", "Gte")]);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule.conditions[0]");
    }

    [Fact]
    public void Build_EmptyGroup_Fails()
    {
        RuleGroupModel model = new(RuleLogic.And, false);

        AssertFailsAt(_factory.Build(RuleSubjectType.ProductRow, model), "rule");
    }

    [Fact]
    public void Build_AccumulatesMultipleErrors()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions:
        [
            Cmp("no.such.field", "Eq", "1"),
            Cmp("line.weeks", "Gte", "lots")
        ]);

        Result<RuleNode> result = _factory.Build(RuleSubjectType.ProductRow, model);

        Assert.True(result.IsFailed);
        ValidationError error = Assert.IsType<ValidationError>(Assert.Single(result.Errors));
        Assert.Equal(2, error.Errors.Count);
    }

    [Fact]
    public void Build_HasValueOperator_Works()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [new RuleConditionModel("line.agentId", "HasValue")]);

        Result<RuleNode> result = _factory.Build(RuleSubjectType.ProductRow, model);

        Assert.True(result.IsSuccess);
        RuleFact withAgent = new(RuleSubjectType.ProductRow, new Dictionary<string, object?> { ["line.agentId"] = 5 });
        RuleFact withoutAgent = new(RuleSubjectType.ProductRow, new Dictionary<string, object?>());
        Assert.True(result.Value.IsSatisfiedBy(withAgent));
        Assert.False(result.Value.IsSatisfiedBy(withoutAgent));
    }

    [Fact]
    public void BuildScriptJson_RoundTripsThroughTheAst()
    {
        RuleGroupModel model = new(RuleLogic.And, false, Conditions: [In("product.id", "In", "7", "15")]);

        Result<string> result = _factory.BuildScriptJson(RuleSubjectType.ProductRow, model);

        Assert.True(result.IsSuccess);
        RuleNode restored = RuleSpecJson.Deserialize(result.Value);
        RuleFact match = new(RuleSubjectType.ProductRow, new Dictionary<string, object?> { ["product.id"] = 7 });
        Assert.True(restored.IsSatisfiedBy(match));
    }

    private static void AssertFailsAt(Result<RuleNode> result, string expectedKey)
    {
        Assert.True(result.IsFailed);
        ValidationError error = Assert.IsType<ValidationError>(Assert.Single(result.Errors));
        Assert.Contains(expectedKey, error.Errors.Keys);
    }
}
