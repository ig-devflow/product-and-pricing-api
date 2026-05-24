using ProductsAndPricingNew.Domain.Entities.Products;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;

namespace ProductsAndPricingNew.UnitTests.Pricing.Specifications;

public sealed class RuleSpecificationTests
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
    public void Interpreter_DiscountExample_MatchesEligibleCourseLine()
    {
        Assert.True(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 5, 1))));
    }

    [Fact]
    public void Interpreter_DiscountExample_MatchesEligibleTransferLine()
    {
        Assert.True(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Transfer, 42, "MT", 5, new DateOnly(2026, 5, 11))));
    }

    [Fact]
    public void Interpreter_DiscountExample_RejectsExcludedAgent()
    {
        Assert.False(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Course, 15, "EG", 102, new DateOnly(2026, 5, 1))));
    }

    [Fact]
    public void Interpreter_DiscountExample_RejectsExcludedTransfer()
    {
        Assert.False(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Transfer, 9, "EG", 5, new DateOnly(2026, 5, 1))));
    }

    [Fact]
    public void Interpreter_DiscountExample_RejectsCourseNotInList()
    {
        Assert.False(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Course, 99, "EG", 5, new DateOnly(2026, 5, 1))));
    }

    [Fact]
    public void Interpreter_DiscountExample_RejectsLateBooking()
    {
        Assert.False(DiscountExample().IsSatisfiedBy(
            Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 6, 1))));
    }

    [Fact]
    public void Interpreter_MissingField_ComparisonIsFalse()
    {
        RuleNode spec = RuleSpec.Field("student.age").Gte(18);

        Assert.False(spec.IsSatisfiedBy(Empty()));
    }

    [Fact]
    public void Interpreter_NotOverInWithMissingField_IsTrue()
    {
        // "not agent X" must hold for a line that has no agent at all.
        RuleNode spec = RuleSpec.Not(RuleSpec.Field("line.agentId").In(1, 2, 3));

        Assert.True(spec.IsSatisfiedBy(Empty()));
    }

    [Theory]
    [InlineData(3, true)]
    [InlineData(2, true)]
    [InlineData(1, false)]
    public void Interpreter_Gte_Numeric(int weeks, bool expected)
    {
        RuleNode spec = RuleSpec.Field("line.weeks").Gte(2);
        RuleFact fact = new(RuleSubjectType.ProductRow,
            new Dictionary<string, object?> { ["line.weeks"] = weeks });

        Assert.Equal(expected, spec.IsSatisfiedBy(fact));
    }

    [Fact]
    public void Interpreter_HasValue()
    {
        RuleNode spec = RuleSpec.Field("promo.code").HasValue();

        Assert.True(spec.IsSatisfiedBy(new(RuleSubjectType.ProductRow,
            new Dictionary<string, object?> { ["promo.code"] = "SUMMER" })));
        Assert.False(spec.IsSatisfiedBy(new(RuleSubjectType.ProductRow,
            new Dictionary<string, object?> { ["promo.code"] = null })));
    }

    [Fact]
    public void Json_RoundTrip_PreservesBehaviour()
    {
        RuleNode spec = DiscountExample();

        string json = RuleSpecJson.Serialize(spec);
        RuleNode restored = RuleSpecJson.Deserialize(json);

        Assert.True(restored.IsSatisfiedBy(Line(ProductKind.Course, 15, "EG", 5, new DateOnly(2026, 5, 1))));
        Assert.False(restored.IsSatisfiedBy(Line(ProductKind.Transfer, 9, "EG", 5, new DateOnly(2026, 5, 1))));
        Assert.Equal(json, RuleSpecJson.Serialize(restored));
    }

    [Fact]
    public void Json_RoundTrip_AllValueTypes()
    {
        RuleNode spec = RuleSpec.And(
            RuleSpec.Field("a.string").Eq("x"),
            RuleSpec.Field("a.int").Eq(7),
            RuleSpec.Field("a.decimal").Gte(9.5m),
            RuleSpec.Field("a.date").Lte(new DateOnly(2026, 1, 1)),
            RuleSpec.Field("a.bool").Eq(true),
            RuleSpec.Field("a.enum").Eq(ProductKind.AddOn));

        string json = RuleSpecJson.Serialize(spec);
        RuleNode restored = RuleSpecJson.Deserialize(json);

        Assert.Equal(json, RuleSpecJson.Serialize(restored));
    }

    private static RuleFact Empty() =>
        new(RuleSubjectType.ProductRow, new Dictionary<string, object?>());
}
