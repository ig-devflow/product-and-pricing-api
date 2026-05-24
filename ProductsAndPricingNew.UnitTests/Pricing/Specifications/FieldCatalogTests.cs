using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Ast;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

namespace ProductsAndPricingNew.UnitTests.Pricing.Specifications;

public sealed class FieldCatalogTests
{
    private readonly FieldCatalog _catalog = new();

    [Fact]
    public void GetFields_ProductRow_ReturnsFieldsForThatSubject()
    {
        IReadOnlyList<FieldDescriptor> fields = _catalog.GetFields(RuleSubjectType.ProductRow);

        Assert.NotEmpty(fields);
        Assert.All(fields, f => Assert.Equal(RuleSubjectType.ProductRow, f.Subject));
        Assert.Contains(fields, f => f.Key == "product.id");
    }

    [Fact]
    public void GetFields_UnknownSubject_ReturnsEmptyNotNull()
    {
        IReadOnlyList<FieldDescriptor> fields = _catalog.GetFields((RuleSubjectType)999);

        Assert.NotNull(fields);
        Assert.Empty(fields);
    }

    [Fact]
    public void Find_ExistingKey_ReturnsDescriptor()
    {
        FieldDescriptor? field = _catalog.Find(RuleSubjectType.ProductRow, "line.weeks");

        Assert.NotNull(field);
        Assert.Equal(RuleValueType.Int, field.DataType);
    }

    [Fact]
    public void Find_UnknownKey_ReturnsNull()
    {
        Assert.Null(_catalog.Find(RuleSubjectType.ProductRow, "no.such.field"));
    }

    [Fact]
    public void Find_KeyFromWrongSubject_ReturnsNull()
    {
        // "product.id" is a ProductRow field; it is not registered for Booking.
        Assert.Null(_catalog.Find(RuleSubjectType.Booking, "product.id"));
    }

    [Fact]
    public void EnumField_ExposesEnumValues()
    {
        FieldDescriptor field = _catalog.Find(RuleSubjectType.ProductRow, "product.kind")!;

        Assert.Equal(RuleValueType.Enum, field.DataType);
        Assert.NotNull(field.EnumValues);
        Assert.Contains("Course", field.EnumValues);
        Assert.Contains("Transfer", field.EnumValues);
    }

    [Fact]
    public void NumericField_AllowsRangeAndInOperators()
    {
        FieldDescriptor field = _catalog.Find(RuleSubjectType.ProductRow, "student.age")!;

        Assert.Contains(RuleOperator.Gte, field.AllowedOperators);
        Assert.Contains(RuleOperator.In, field.AllowedOperators);
    }

    [Fact]
    public void DateField_AllowsRangeButNotIn()
    {
        FieldDescriptor field = _catalog.Find(RuleSubjectType.ProductRow, "booking.dateBooked")!;

        Assert.Contains(RuleOperator.Lte, field.AllowedOperators);
        Assert.DoesNotContain(RuleOperator.In, field.AllowedOperators);
    }
}
