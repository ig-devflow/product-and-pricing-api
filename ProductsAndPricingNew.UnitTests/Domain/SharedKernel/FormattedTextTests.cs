using ProductsAndPricingNew.Domain.Common.Exceptions;
using ProductsAndPricingNew.Domain.ReferenceData;
using ProductsAndPricingNew.Domain.SharedKernel.ValueObjects;

namespace ProductsAndPricingNew.UnitTests.Domain.SharedKernel;

public sealed class FormattedTextTests
{
    [Fact]
    public void Create_WithNullOrWhitespaceAndFormatNone_ReturnsEmpty()
    {
        FormattedText nullText = FormattedText.Create(null, ContentFormat.None);
        FormattedText whitespaceText = FormattedText.Create(" ", ContentFormat.None);

        Assert.Same(FormattedText.Empty, nullText);
        Assert.Same(FormattedText.Empty, whitespaceText);
    }

    [Fact]
    public void Create_WithContentAndNoneFormat_Throws()
    {
        Assert.Throws<DomainException>(() => FormattedText.Create("Text", ContentFormat.None));
    }

    [Fact]
    public void Create_WithNoContentAndPlainTextFormat_Throws()
    {
        Assert.Throws<DomainException>(() => FormattedText.Create(" ", ContentFormat.PlainText));
    }

    [Fact]
    public void Create_WithInvalidFormat_Throws()
    {
        Assert.Throws<DomainException>(() => FormattedText.Create("Text", (ContentFormat)999));
    }

    [Fact]
    public void Create_WithPlainText_ReturnsNormalizedText()
    {
        FormattedText text = FormattedText.Create("  Hello   world  ", ContentFormat.PlainText);

        Assert.Equal("Hello world", text.Content);
        Assert.Equal(ContentFormat.PlainText, text.Format);
    }

    [Fact]
    public void Create_WithHtml_ReturnsHtmlText()
    {
        FormattedText text = FormattedText.Create("<p>Hello</p>", ContentFormat.Html);

        Assert.Equal("<p>Hello</p>", text.Content);
        Assert.Equal(ContentFormat.Html, text.Format);
    }
}
