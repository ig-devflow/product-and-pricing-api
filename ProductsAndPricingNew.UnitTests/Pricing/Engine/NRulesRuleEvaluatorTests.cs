using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Pricing.Compilation;
using ProductsAndPricingNew.Pricing.Engine;

namespace ProductsAndPricingNew.UnitTests.Pricing.Engine;

public sealed class NRulesRuleEvaluatorTests
{
    private static NRulesRuleEvaluator NewEvaluator() =>
        new(new RulesetCompiler(), new RuleSessionCache());

    private static string Script(RuleNode node) => RuleSpecJson.Serialize(node);

    private static RuleFact Line(ProductKind kind, int productId) =>
        new(RuleSubjectType.ProductRow, new Dictionary<string, object?>
        {
            ["product.kind"] = kind,
            ["product.id"] = productId
        });

    [Fact]
    public void Evaluate_MatchesOnlyEligibleFacts()
    {
        Ruleset ruleset = Ruleset.Create("Discounts", RuleSubjectType.ProductRow, divisionId: 7);
        ruleset.AddRule(
            "Course discount",
            Script(RuleSpec.And(
                RuleSpec.Field("product.kind").Eq(ProductKind.Course),
                RuleSpec.Field("product.id").In(7, 15, 76, 81))),
            priority: 10);

        RuleFact eligible = Line(ProductKind.Course, 15);
        RuleFact wrongProduct = Line(ProductKind.Course, 99);
        RuleFact wrongKind = Line(ProductKind.Transfer, 15);

        IReadOnlyList<RuleMatch> matches = NewEvaluator()
            .Evaluate([ruleset], [eligible, wrongProduct, wrongKind]);

        RuleMatch match = Assert.Single(matches);
        Assert.Equal("Course discount", match.RuleName);
        Assert.Same(eligible, match.Fact);
    }

    [Fact]
    public void Evaluate_FiresEveryRuleAgainstEveryMatchingFact()
    {
        Ruleset ruleset = Ruleset.Create("Discounts", RuleSubjectType.ProductRow, divisionId: 7);
        ruleset.AddRule("Courses", Script(RuleSpec.Field("product.kind").Eq(ProductKind.Course)), 10);
        ruleset.AddRule("Transfers", Script(RuleSpec.Field("product.kind").Eq(ProductKind.Transfer)), 20);

        RuleFact course = Line(ProductKind.Course, 1);
        RuleFact transfer = Line(ProductKind.Transfer, 2);

        IReadOnlyList<RuleMatch> matches = NewEvaluator().Evaluate([ruleset], [course, transfer]);

        Assert.Equal(2, matches.Count);
        Assert.Contains(matches, m => m.RuleName == "Courses" && ReferenceEquals(m.Fact, course));
        Assert.Contains(matches, m => m.RuleName == "Transfers" && ReferenceEquals(m.Fact, transfer));
    }

    [Fact]
    public void Evaluate_NoMatchingFacts_ReturnsEmpty()
    {
        Ruleset ruleset = Ruleset.Create("Discounts", RuleSubjectType.ProductRow, divisionId: 7);
        ruleset.AddRule("Courses", Script(RuleSpec.Field("product.kind").Eq(ProductKind.Course)), 10);

        IReadOnlyList<RuleMatch> matches = NewEvaluator().Evaluate([ruleset], [Line(ProductKind.Transfer, 1)]);

        Assert.Empty(matches);
    }

    [Fact]
    public void Evaluate_NoFacts_ReturnsEmpty()
    {
        Ruleset ruleset = Ruleset.Create("Discounts", RuleSubjectType.ProductRow, divisionId: 7);
        ruleset.AddRule("Courses", Script(RuleSpec.Field("product.kind").Eq(ProductKind.Course)), 10);

        Assert.Empty(NewEvaluator().Evaluate([ruleset], []));
    }
}
