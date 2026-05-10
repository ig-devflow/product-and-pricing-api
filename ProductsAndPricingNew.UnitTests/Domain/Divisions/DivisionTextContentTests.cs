using ProductsAndPricingNew.Domain.Entities.PricingRef;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.TextContent;

namespace ProductsAndPricingNew.UnitTests.Domain.Divisions;

public sealed class DivisionTextContentTests
{
    [Fact]
    public void ReplaceTexts_CreatesActiveChildEntity()
    {
        Division division = new Division.Builder("Division", "https://example.com")
            .Texts([new TextContentDefinition(100, 10, "Audience text", ContentFormat.PlainText)])
            .Build();

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.Equal(100, text.ContentTemplateId);
        Assert.Equal(10, text.AudienceId);
        Assert.Equal("Audience text", text.Text.Content);
        Assert.False(text.IsDeleted);
    }

    [Fact]
    public void ReplaceTexts_EmptyExistingTextSoftDeletesChildEntity()
    {
        Division division = new Division.Builder("Division", "https://example.com")
            .Texts([new TextContentDefinition(100, 10, "Audience text", ContentFormat.PlainText)])
            .Build();

        division.ReplaceTexts([new TextContentDefinition(100, 10, null, ContentFormat.None)]);

        DivisionTextContent text = Assert.Single(division.Texts);
        Assert.True(text.IsDeleted);
    }
}
