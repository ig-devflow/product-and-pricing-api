using FluentResults;
using ProductsAndPricingNew.Application.Features.Pricing.Models;
using ProductsAndPricingNew.Application.Features.Pricing.Queries.GetFieldCatalog;
using ProductsAndPricingNew.Domain.Entities.Rules;
using ProductsAndPricingNew.Domain.Pricing.Specifications.Catalog;

namespace ProductsAndPricingNew.UnitTests.Application.Pricing;

public sealed class GetFieldCatalogQueryHandlerTests
{
    private readonly GetFieldCatalogQueryHandler _handler = new(new FieldCatalog());

    [Fact]
    public async Task Handle_ReturnsMappedFieldsForSubject()
    {
        Result<IReadOnlyCollection<FieldDescriptorDto>> result =
            await _handler.Handle(new GetFieldCatalogQuery(RuleSubjectType.ProductRow), default);

        Assert.True(result.IsSuccess);
        Assert.NotEmpty(result.Value);

        FieldDescriptorDto productId = Assert.Single(result.Value, f => f.Key == "product.id");
        Assert.Equal("Int", productId.DataType);
        Assert.Equal("Products", productId.ValueSource);
        Assert.Contains("In", productId.AllowedOperators);
    }

    [Fact]
    public async Task Handle_EnumField_IncludesEnumValues()
    {
        Result<IReadOnlyCollection<FieldDescriptorDto>> result =
            await _handler.Handle(new GetFieldCatalogQuery(RuleSubjectType.ProductRow), default);

        FieldDescriptorDto kind = Assert.Single(result.Value, f => f.Key == "product.kind");
        Assert.Equal("Enum", kind.DataType);
        Assert.NotNull(kind.EnumValues);
        Assert.Contains("Course", kind.EnumValues);
    }
}
