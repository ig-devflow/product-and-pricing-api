using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Pricing.Compilation;

namespace ProductsAndPricingNew.UnitTests.Pricing.Compilation;

public sealed class RuleNodeExpressionCompilerTests
{
    // Discount: booked <= 2026-05-11; Courses {7,15,76,81} OR Transfers (all except {6,9,55});
    // sale country EG/MT; NOT agents {17,8,102,104}.
    private static RuleNode DiscountExample() => RuleSpec.And(
        RuleSpec.Field("booking.dateBooked").Lte(new DateOnly(2026, 5, 11)),
        RuleSpec.Or(
            RuleSpec.And(
                RuleSpec.Field("product.kind").Eq(ProductKind.Course),
                RuleSpec.Field("product.id").In(7, 15, 76, 81)),
            RuleSpec.And(
                RuleSpec.Field("product.kind").Eq(ProductKind.Transfer),
                RuleSpec.Not(RuleSpec.Field("product.id").In(6, 9, 55)))),
        RuleSpec.Field("line.saleCountry").In("EG", "MT"),
        RuleSpec.Not(RuleSpec.Field("line.agentId").In(17, 8, 102, 104)));

    private static RuleFact Line(ProductKind kind, int productId, string country, int agentId, DateOnly booked) =>
        new(RuleSubjectType.ProductRow, new Dictionary<string, object?>
        {
            ["booking.dateBooked"] = booked,
            ["product.kind"] = kind,
            ["product.id"] = productId,
            ["line.saleCountry"] = country,
            ["line.agentId"] = agentId
        });

    [Fact]
    public void Compiled_DiscountExample_AgreesWithInterpreterOnEveryFact()
    {
        RuleNode spec = DiscountExample();
        Func<RuleFact, bool> compiled = RuleNodeExpressionCompiler.Compile(spec).Compile();

        RuleFact[] facts =
        [
            Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 5, 1)),
            Line(ProductKind.Transfer, 42, "MT", 5, new DateOnly(2026, 5, 11)),
            Line(ProductKind.Course, 99, "EG", 5, new DateOnly(2026, 5, 1)),
            Line(ProductKind.Transfer, 9, "EG", 5, new DateOnly(2026, 5, 1)),
            Line(ProductKind.Course, 15, "EG", 102, new DateOnly(2026, 5, 1)),
            Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 6, 1))
        ];

        foreach (RuleFact fact in facts)
            Assert.Equal(spec.IsSatisfiedBy(fact), compiled(fact));
    }

    [Fact]
    public void Compiled_DiscountExample_AcceptsEligibleLine()
    {
        Func<RuleFact, bool> compiled = RuleNodeExpressionCompiler.Compile(DiscountExample()).Compile();

        Assert.True(compiled(Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 5, 1))));
    }

    [Fact]
    public void Compiled_DiscountExample_RejectsExcludedAgent()
    {
        Func<RuleFact, bool> compiled = RuleNodeExpressionCompiler.Compile(DiscountExample()).Compile();

        Assert.False(compiled(Line(ProductKind.Course, 15, "EG", 102, new DateOnly(2026, 5, 1))));
    }

    [Theory]
    [InlineData(3, true)]
    [InlineData(2, true)]
    [InlineData(1, false)]
    public void Compiled_RangeComparison(int weeks, bool expected)
    {
        Func<RuleFact, bool> compiled =
            RuleNodeExpressionCompiler.Compile(RuleSpec.Field("line.weeks").Gte(2)).Compile();
        RuleFact fact = new(RuleSubjectType.ProductRow, new Dictionary<string, object?> { ["line.weeks"] = weeks });

        Assert.Equal(expected, compiled(fact));
    }

    [Fact]
    public void Compiled_HasValue()
    {
        Func<RuleFact, bool> compiled =
            RuleNodeExpressionCompiler.Compile(RuleSpec.Field("promo.code").HasValue()).Compile();

        Assert.True(compiled(new(RuleSubjectType.ProductRow,
            new Dictionary<string, object?> { ["promo.code"] = "SUMMER" })));
        Assert.False(compiled(new(RuleSubjectType.ProductRow,
            new Dictionary<string, object?>())));
    }

    [Fact]
    public void Compiled_MissingFieldComparison_IsFalse()
    {
        Func<RuleFact, bool> compiled =
            RuleNodeExpressionCompiler.Compile(RuleSpec.Field("student.age").Gte(18)).Compile();

        Assert.False(compiled(new(RuleSubjectType.ProductRow, new Dictionary<string, object?>())));
    }
}
